# RabbitMQ Tutorial - Reliable Publishing with Publisher Confirms

## Publisher Confirms (using Steeltoe)

> #### Prerequisites
> This tutorial assumes RabbitMQ is [downloaded](https://www.rabbitmq.com/download.html) and installed and running 
> on `localhost` on the [standard port](https://www.rabbitmq.com/networking.html#ports) (`5672`). 
> 
> In case you use a different host, port or credentials, connections settings would require adjusting.
>
> #### Where to get help
> If you're having trouble going through this tutorial you can contact us through Github issues on our
> [Steeltoe Samples Repository](https://github.com/SteeltoeOSS/Samples).

[Publisher confirms](https://www.rabbitmq.com/confirms.html#publisher-confirms)
are a RabbitMQ extension to implement reliable
publishing. When publisher confirms are enabled on a channel,
messages the client publishes are confirmed asynchronously
by the broker, meaning they have been taken care of on the server
side.

### Overview

In this tutorial we're going to use publisher confirms to make
sure published messages have safely reached the broker.  We will cover several strategies to using publisher confirms and explain their pros and cons.


### Enabling Publisher Confirms on a Channel

Publisher confirms are a RabbitMQ extension to the AMQP 0.9.1 protocol,
so they are not enabled by default. Publisher confirms are
enabled at the channel level of a connection to the RabbitMQ broker.

Remember from the first tutorial we explained that Steeltoe adds to the service container a Caching zvonnection Factory that is used to create and cache connections to the RabbitMQ broker. By default, all of the Steeltoe RabbitMQ components (e.g. RabbitTemplate) use the factory when interacting with the broker (i.e. creating connections and channels). 

By default the factory does not create connections/channels that have publisher confirms enabled. So in order to use publisher confirms in Steeltoe we need to add an additional connection factory to the service container configured with publisher confirms enabled.  And we also then need to add an additional `RabbitTemplate` that is configured to use this additional connection factory.

We do that when configuring the services in the `RabbitMQHost` as follows:

```csharp
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Connection;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Host;
namespace Sender
{
    public class Program
    {
        internal const string QueueName = "hello";
        public static void Main(string[] args)
        {
            RabbitMQHost.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    // Add queue to service container to be declared
                    services.AddRabbitQueue(new Queue(QueueName));

                    // Add a connection factory with the name "publisherConfirmReturnsFactory"
                    // and configure it to use correlated publisher confirms
                    services.AddRabbitConnectionFactory("publisherConfirmReturnsFactory", (p, ccf) =>
                    {
                        ccf.IsPublisherReturns = true;
                        ccf.PublisherConfirmType = CachingConnectionFactory.ConfirmType.CORRELATED;
                    });

                    // Add an additional RabbitTemplate with the name "confirmReturnsTemplate"
                    // and configure it to use the above connection factory and mandatory delivery
                    services.AddRabbitTemplate("confirmReturnsTemplate", (p, template) =>
                    {
                        var ccf = p.GetRabbitConnectionFactory("publisherConfirmReturnsFactory");
                        template.ConnectionFactory = ccf;
                        template.Mandatory = true;
                    });

                    services.AddHostedService<Tut7Sender>();
                })
                .Build()
                .Run();
        }
    }
}
```

Once this is done, then in our Sender we can use the following code to obtain the named `RabbitTemplate` which we configured properly above.

```csharp
namespace Sender
{
    public class Tut7Sender : BackgroundService, IReturnCallback, IConfirmCallback
    {
        private readonly ILogger<Tut7Sender> _logger;
        private readonly RabbitTemplate _rabbitTemplate;

        public Tut7Sender(ILogger<Tut7Sender> logger, IServiceProvider provider)
        {
            _logger = logger;
            _rabbitTemplate = provider.GetRabbitTemplate("confirmReturnsTemplate");

            ....
        }
    }
}

```

### Strategy #1: Publishing Messages Individually

Let's start with the simplest approach to publishing with confirms,
that is, publishing a message and waiting synchronously for its confirmation:

```csharp 
while (ThereAreMessagesToPublish()) {
    ....
     _rabbitTemplate.ConvertAndSend(QueueName, (object)"Hello World!");

    // Wait up to 5 seconds for confirmation
    _rabbitTemplate.WaitForConfirmsOrDie(5000);
}
```

In the previous example we publish a message as usual and wait for its
confirmation with the `Channel#WaitForConfirmsOrDie(long)` method.
The method returns as soon as the message has been confirmed. If the
message is not confirmed within the timeout or if it is nack-ed (meaning
the broker could not take care of it for some reason), the method will
throw an exception. The handling of the exception usually consists
in logging an error message and/or retrying to send the message.

This technique is very straightforward but also has a major drawback:
it **significantly slows down publishing**, as the confirmation of a message blocks the publishing
of all subsequent messages. This approach is not going to deliver throughput of
more than a few hundreds of published messages per second. Nevertheless, this can be
good enough for some applications.

> #### Are Publisher Confirms Asynchronous?
>
> We mentioned at the beginning that the broker confirms published
> messages asynchronously but in the first example the code waits
> synchronously until the message is confirmed. The client actually
> receives confirms asynchronously and unblocks the call to `WaitForConfirmsOrDie`
> accordingly. Think of `WaitForConfirmsOrDie` as a synchronous helper
> which relies on asynchronous notifications under the hood.


### Strategy #2: Publishing Messages in Batches

To improve upon our previous example, we can publish a batch
of messages and wait for this whole batch to be confirmed.
The following example uses a batch of 100:

```csharp
int batchSize = 100;
int outstandingMessageCount = 0;
while (thereAreMessagesToPublish()) {
     _rabbitTemplate.ConvertAndSend(QueueName, (object)"Hello World!");
    outstandingMessageCount++;
    if (outstandingMessageCount == batchSize) {
        _rabbitTemplate.WaitForConfirmsOrDie(5000);
        outstandingMessageCount = 0;
    }
}
if (outstandingMessageCount > 0) {
    _rabbitTemplate.WaitForConfirmsOrDie(5000);
}
```

Waiting for a batch of messages to be confirmed improves throughput drastically over
waiting for a confirm for individual message (up to 20-30 times with a remote RabbitMQ node).
One drawback is that we do not know exactly what went wrong in case of failure,
so we may have to keep a whole batch in memory to log something meaningful or
to re-publish the messages. And this solution is still synchronous, so it
blocks the publishing of messages.


### Strategy #3: Handling Publisher Confirms Asynchronously

The broker confirms published messages asynchronously, one just needs
to register a callback with the template to be notified of these confirms or returns.

Here is a simple example of how to do that:

```csharp

using Steeltoe.Messaging;
using Steeltoe.Messaging.RabbitMQ.Connection;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using static Steeltoe.Messaging.RabbitMQ.Core.RabbitTemplate;

namespace Sender
{
    public class Tut7Sender : BackgroundService, IReturnCallback, IConfirmCallback
    {
        private readonly ILogger<Tut7Sender> _logger;
        private readonly RabbitTemplate _rabbitTemplate;
        private int id;

        public Tut7Sender(ILogger<Tut7Sender> logger, IServiceProvider provider)
        {
            _logger = logger;
            _rabbitTemplate = provider.GetRabbitTemplate("confirmReturnsTemplate");
            _rabbitTemplate.ReturnCallback = this;
            _rabbitTemplate.ConfirmCallback = this;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                CorrelationData data = new CorrelationData(id.ToString());
                id++;
                await _rabbitTemplate.ConvertAndSendAsync(Program.QueueName, (object)"Hello World!", data);
                _logger.LogInformation("Worker running at: {time}, sent ID: {id}", DateTimeOffset.Now, data.Id);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public void ReturnedMessage(IMessage<byte[]> message, int replyCode, string replyText, string exchange, string routingKey)
        {
            _logger.LogInformation($"Returned message: ReplyCode={replyCode}, Exchange={exchange}, RoutingKey={routingKey}");
        }

        public void Confirm(CorrelationData correlationData, bool ack, string cause)
        {
            _logger.LogInformation($"Confirming message: Id={correlationData.Id}, Acked={ack}, Cause={cause}");
        }
    }
}
```

There are two callbacks: one for confirmed messages and one returned messages. 

```csharp

.....
public void ReturnedMessage(IMessage<byte[]> message, int replyCode, string replyText, string exchange, string routingKey)
{
    _logger.LogInformation($"Returned message: ReplyCode={replyCode}, Exchange={exchange}, RoutingKey={routingKey}");
}

public void Confirm(CorrelationData correlationData, bool ack, string cause)
{
    _logger.LogInformation($"Confirming message: Id={correlationData.Id}, Acked={ack}, Cause={cause}");
}
....

```

For the `ReturnedMessage()` callback method to be 
invoked the templates property `Mandatory` must be set to true and the underlying connection factory must be configured
with `IsPublisherReturns` set to true.  If those values are set, then the template will issue the returns callback to whatever is registered
with the template property `ReturnCallback`.

For publisher confirms (also known as publisher acknowledgements) to be enabled, the template requires the underlying connection factory 
to have `PublisherConfirmType` property set to `ConfirmType.CORRELATED`. Then the template will issue confirm callbacks to whatever is registered 
with the template property `ConfirmCallback.`

Note that the `CorrelationData` provided in the `Confirm(CorrelationData correlationData, ...)` is provided the user (developer) on the `ConvertAndSendAsync(...)` method call. 
The template then returns it as part of the arguments to the `Confirm(...)` callback.

```csharp

....
CorrelationData data = new CorrelationData(id.ToString());
id++;
await _rabbitTemplate.ConvertAndSendAsync(Program.QueueName, (object)"Hello World!", data);
...

```

A simple way to correlate messages with sequence numbering consists in using a
dictionary of `CorrelationData` and messages . The publishing code can then track outbound 
messages using the dictionary and upon receiving the `Confirm` callback can behave accordingly
depending on whether the message was acked or nacked.

### Summary

Making sure published messages made it to the broker can be essential in some applications.
Publisher confirms are a RabbitMQ feature that helps to meet this requirement. Publisher
confirms are asynchronous in nature but it is also possible to handle them synchronously.
There is no definitive way to implement publisher confirms, this usually comes down
to the constraints in the application and in the overall system. Typical techniques are:

 * publishing messages individually, waiting for the confirmation synchronously: simple, but very
 limited throughput.
 * publishing messages in batch, waiting for the confirmation synchronously for a batch: simple, reasonable
 throughput, but hard to reason about when something goes wrong.
 * asynchronous handling: best performance and use of resources, good control in case of error, but
 can be involved to implement correctly.

## Putting It All Together

The code for the receivers `Program.cs` comes from the first tutorial:

```csharp

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Host;
namespace Receiver
{
    internal class Program
    {
        internal const string QueueName = "hello";

        static void Main(string[] args)
        {
            RabbitMQHost.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Add queue to service container to be declared
                    services.AddRabbitQueue(new Queue(QueueName));

                    // Add the rabbit listener
                    services.AddSingleton<Tut7Receiver>();
                    services.AddRabbitListeners<Tut7Receiver>();
                })
                .Build()
                .Run();
        }
    }
}
```

The code for the `Tut7Receiver` also looks the same as in tutorial one:

```csharp
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace Receiver
{
    internal class Tut7Receiver
    {
        private readonly ILogger _logger;

        public Tut7Receiver(ILogger<Tut7Receiver> logger)
        {
            _logger = logger;
        }

        [RabbitListener(Queue = Program.QueueName)]
        public void Receive(string input)
        {
            _logger.LogInformation($"Received: {input}");
        }
    }
}
```

The code for the Sender is also based on tutorial one, but has the modifications for confirms.

The senders `Program.cs` is as follows:

```csharp
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Connection;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Host;
namespace Sender
{
    public class Program
    {
        internal const string QueueName = "hello";
        public static void Main(string[] args)
        {
            RabbitMQHost.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    // Add queue to service container to be declared
                    services.AddRabbitQueue(new Queue(QueueName));

                    services.AddRabbitConnectionFactory("publisherConfirmReturnsFactory", (p, ccf) =>
                    {
                        ccf.IsPublisherReturns = true;
                        ccf.PublisherConfirmType = CachingConnectionFactory.ConfirmType.CORRELATED;
                    });

                    services.AddRabbitTemplate("confirmReturnsTemplate", (p, template) =>
                    {
                        var ccf = p.GetRabbitConnectionFactory("publisherConfirmReturnsFactory");
                        template.ConnectionFactory = ccf;
                        template.Mandatory = true;
                    });

                    services.AddHostedService<Tut7Sender>();
                })
                .Build()
                .Run();
        }
    }
}
```

The senders background service looks as follows:

```csharp
using Steeltoe.Messaging;
using Steeltoe.Messaging.RabbitMQ.Connection;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using static Steeltoe.Messaging.RabbitMQ.Core.RabbitTemplate;

namespace Sender
{
    public class Tut7Sender : BackgroundService, IReturnCallback, IConfirmCallback
    {
        private readonly ILogger<Tut7Sender> _logger;
        private readonly RabbitTemplate _rabbitTemplate;
        private int id;

        public Tut7Sender(ILogger<Tut7Sender> logger, IServiceProvider provider)
        {
            _logger = logger;
            _rabbitTemplate = provider.GetRabbitTemplate("confirmReturnsTemplate");
            _rabbitTemplate.ReturnCallback = this;
            _rabbitTemplate.ConfirmCallback = this;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                CorrelationData data = new CorrelationData(id.ToString());
                id++;
                await _rabbitTemplate.ConvertAndSendAsync(Program.QueueName, (object)"Hello World!", data);
                _logger.LogInformation("Worker running at: {time}, sent ID: {id}", DateTimeOffset.Now, data.Id);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public void ReturnedMessage(IMessage<byte[]> message, int replyCode, string replyText, string exchange, string routingKey)
        {
            _logger.LogInformation($"Returned message: ReplyCode={replyCode}, Exchange={exchange}, RoutingKey={routingKey}");
        }

        public void Confirm(CorrelationData correlationData, bool ack, string cause)
        {
            _logger.LogInformation($"Confirming message: Id={correlationData.Id}, Acked={ack}, Cause={cause}");
        }
    }
}
```

Compile as usual, see [tutorial one](../Tutorial1/Readme.md)

```bash
cd tutorials\tutorial7
dotnet build
```

To run the receiver, execute the following commands:

```bash
# receiver

cd receiver
dotnet run
```

To watch the confirms come back, run the sender

```bash
# sender

cd sender
dotnet run
```
