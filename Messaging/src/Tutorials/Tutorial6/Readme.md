# RabbitMQ Tutorial - Remote procedure call (RPC)

## Remote procedure call (using Steeltoe)

> #### Prerequisites
> This tutorial assumes RabbitMQ is [downloaded](https://www.rabbitmq.com/download.html) and installed and running 
> on `localhost` on the [standard port](https://www.rabbitmq.com/networking.html#ports) (`5672`). 
> 
> In case you use a different host, port or credentials, connections settings would require adjusting.
>
> #### Where to get help
> If you're having trouble going through this tutorial you can contact us through Github issues on our
> [Steeltoe Samples Repository](https://github.com/SteeltoeOSS/Samples).


In the [second tutorial](../Tutorial2/readme.md) we learned how to
use _Work Queues_ to distribute time-consuming tasks among multiple
workers.

But what if we need to run a function on a remote computer and wait for
the result? Well, that's a different story. This pattern is commonly
known as _Remote Procedure Call_ or _RPC_.

In this tutorial we're going to use RabbitMQ to build an RPC system: a
client and a scalable RPC server. As we don't have any time-consuming
tasks that are worth distributing, we're going to create a dummy RPC
service that returns Fibonacci numbers.

## Client interface

Normally when we talk about RPC's, we talk in terms of an RPC "Client" and "Server".
In the context of sending and receiving, our Sender will become the RPC "Client" and our Receiver will be our RPC "Server".
When the sender calls the server we will get back the fibonacci of the argument we call with.  Here is how the sender will use
the `RabbitTemplate` to invoke the server.

```csharp
  int result = await _rabbitTemplate.ConvertSendAndReceiveAsync<int>(RPCExchangeName, "rpc", start++);
  _logger.LogInformation($"Got result: {result}");
```

> #### A note on RPC
>
> Although RPC is a pretty common pattern in computing, it's often criticised.
> The problems arise when a programmer is not aware
> whether a function call is local or if it's a slow RPC. Confusions
> like that result in an unpredictable system and adds unnecessary
> complexity to debug. Instead of simplifying software, misused RPC
> can result in unmaintainable spaghetti code.
>
> Bearing that in mind, consider the following advice:
>
>  * Make sure it's obvious which function call is local and which is remote.
>  * Document your system. Make the dependencies between components clear.
>  * Handle error cases. How should the client react when the RPC server is
>    down for a long time?
>
> When in doubt avoid RPC. If you can, you should use an asynchronous
> pipeline - instead of RPC-like blocking, results are asynchronously
> pushed to a next computation stage.


## Callback queue

In general doing RPC over RabbitMQ is easy. A client sends a request
message and a server replies with a response message. In order to
receive a response we need to send a 'callback' queue address with the
request. Steeltoe's `RabbitTemplate` handles the callback queue for
us when we use the above `ConvertSendAndReceiveAsync()` method.  There is
no need to do any other setup when using the `RabbitTemplate`. 
For a thorough explanation please see [Request/Reply Message](https://docs.steeltoe.io/api/v3/messaging/rabbitmq-reference.html#request-and-reply-messaging).

> #### Message properties
>
> The AMQP 0-9-1 protocol predefines a set of 14 properties that go with
> a message. Most of the properties are rarely used, with the exception of
> the following:
>
> * `deliveryMode`: Marks a message as persistent (with a value of `2`)
>    or transient (any other value). You may remember this property
>    from [the second tutorial](../Tutorial2/readme.md).
> * `contentType`: Used to describe the mime-type of the encoding.
>    For example for the often used JSON encoding it is a good practice
>    to set this property to: `application/json`.
> * `replyTo`: Commonly used to name a callback queue.
> * `correlationId`: Useful to correlate RPC responses with requests.

## Correlation Id

Steeltoe allows you to focus on the message style you're working
with and hide the details of message plumbing required to support
this style. For example, typically the native client would
create a callback queue for every RPC request. That's pretty
inefficient so an alternative is to create a single callback
queue per client.

That raises a new issue, having received a response in that queue it's
not clear to which request the response belongs. That's when the
`correlationId` property is used. Steeltoe automatically sets
a unique value for every request. In addition it handles the details
of matching the response with the correct correlationID.

One reason that Steeltoe makes RPC style easier over RabbitMQ is that sometimes
you may want to ignore unknown messages in the callback
queue, rather than failing with an error. It's due to a possibility of
a race condition on the server side. Although unlikely, it is possible
that the RPC server will die just after sending us the answer, but
before sending an acknowledgment message for the request. If that
happens, the restarted RPC server will process the request again.
Steeltoe handles the duplicate responses gracefully,
and the RPC should ideally be idempotent.

### Summary

<div class="diagram">
  <img src="../img/tutorials/python-six.png" height="200" alt="Summary illustration, which is described in the following bullet points." />
</div>

Our RPC will work like this:

  * We will setup a new `DirectExchange`
  * The client will leverage the `ConvertSendAndReceive` method, passing the exchange
    name, the routingKey, and the message.
  * The request is sent to an RPC queue `tut.rpc`.
  * The RPC worker (i.e. Server) is waiting for requests on that queue.
    When a request appears, it performs the task and returns a message with the
    result back to the client, using the queue from the `replyTo` field.
  * The client waits for data on the callback queue. When a message
    appears, it checks the `correlationId` property. If it matches
    the value from the request it returns the response to the
    application. Again, this is done automagically via the Steeltoe `RabbitTemplate`.

Putting it all together
-----------------------

The Fibonacci task is a `RabbitListener` and is defined as:

```csharp
  [RabbitListener(Queue = "tut.rpc.requests")]
  // [SendTo("tut.rpc.replies")] used when the client doesn't set replyTo.
  public int Fibonacci(int n)
  {
      _logger.LogInformation($"Received request for {n}");
      var result = Fib(n);
      _logger.LogInformation($"Returning {result}");
      return result;
  }

  private int Fib(int n)
  {
      return n == 0 ? 0 : n == 1 ? 1 : (Fib(n - 1) + Fib(n - 2));
  }
```

We declare our Fibonacci function. It assumes only valid positive integer input.
(Don't expect this one to work for big numbers,
and it's probably the slowest recursive implementation possible).

The code to configure the RabbitMQ entities looks like this:

```csharp
[DeclareQueue(Name = "tut.rpc.requests")]
[DeclareExchange(Name = Program.RPCExchangeName, Type = ExchangeType.DIRECT)]
[DeclareQueueBinding(Name ="binding.rpc.queue.exchange", QueueName = "tut.rpc.requests", ExchangeName = Program.RPCExchangeName, RoutingKey = "rpc")]
```

The server code is rather straightforward:

  * As usual we start annotating our receiver method with a `RabbitListener`
    and defining the RabbitMQ entities using the [Declare****()] attributes
  * Our Fibonacci method calls Fib() with the payload parameter and returns
    the result

The code for our RPC server:

```csharp
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using Steeltoe.Messaging.RabbitMQ.Config;

namespace Receiver
{
    [DeclareQueue(Name = "tut.rpc.requests")]
    [DeclareExchange(Name = Program.RPCExchangeName, Type = ExchangeType.DIRECT)]
    [DeclareQueueBinding(Name ="binding.rpc.queue.exchange", QueueName = "tut.rpc.requests", ExchangeName = Program.RPCExchangeName, RoutingKey = "rpc")]
    internal class Tut6Receiver
    {
        private readonly ILogger _logger;

        public Tut6Receiver(ILogger<Tut6Receiver> logger)
        {
            _logger = logger;
        }

        [RabbitListener(Queue = "tut.rpc.requests")]
        // [SendTo("tut.rpc.replies")] used when the client doesn't set replyTo.
        public int Fibonacci(int n)
        {
            _logger.LogInformation($"Received request for {n}");
            var result = Fib(n);
            _logger.LogInformation($"Returning {result}");
            return result;
        }

        private int Fib(int n)
        {
            return n == 0 ? 0 : n == 1 ? 1 : (Fib(n - 1) + Fib(n - 2));
        }
    }
}
```



The client code is as easy as the server:

  * We inject the `RabbitTemplate` service 
  * We invoke `template.ConvertSendAndReceiveAsync()` with the parameters
    exchange name, routing key and message.
  * We print the result

```csharp
using Steeltoe.Messaging.RabbitMQ.Core;

namespace Sender
{
    public class Tut6Sender : BackgroundService
    {
        internal const string RPCExchangeName = "tut.rpc";
        private readonly ILogger<Tut6Sender> _logger;
        private readonly RabbitTemplate _rabbitTemplate;
        private int start = 0;

        public Tut6Sender(ILogger<Tut6Sender> logger, RabbitTemplate rabbitTemplate)
        {
            _logger = logger;
            _rabbitTemplate = rabbitTemplate;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _logger.LogInformation($"Requesting Fib({start})");
                int result = await _rabbitTemplate.ConvertSendAndReceiveAsync<int>(RPCExchangeName, "rpc", start++);
                _logger.LogInformation($"Got result: {result}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
```

Compile as usual, see [tutorial one](../Tutorial1/Readme.md)

```bash
cd tutorials\tutorial6
dotnet build
```

To run the server, execute the following commands:

```bash
# server

cd receiver
dotnet run
```

To request a fibonacci number run the client:

```bash
# client

cd sender
dotnet run
```

The design presented here is not the only possible implementation of a RPC
service, but it has some important advantages:

 * If the RPC server is too slow, you can scale up by just running
   another one. Try running a second `RPC Server` in a new console.
 * On the client side, the RPC requires sending and
   receiving only one message with one method. No synchronous calls
   like `queueDeclare` are required. As a result the RPC client needs
   only one network round trip for a single RPC request.

Our code is still pretty simplistic and doesn't try to solve more
complex (but important) problems, like:

 * How should the client react if there are no servers running?
 * Should a client have some kind of timeout for the RPC?
 * If the server malfunctions and raises an exception, should it be
   forwarded to the client?
 * Protecting against invalid incoming messages
   (eg checking bounds, type) before processing.

   
Next, find out how to use publisher confirms
in [tutorial 7](../Tutorial7/readme.md)

