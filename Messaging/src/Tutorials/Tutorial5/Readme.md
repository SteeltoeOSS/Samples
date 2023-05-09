# RabbitMQ Tutorial - Topics

## Topics (using Steeltoe)

> #### Prerequisites
> This tutorial assumes RabbitMQ is [downloaded](https://www.rabbitmq.com/download.html) and installed and running 
> on `localhost` on the [standard port](https://www.rabbitmq.com/networking.html#ports) (`5672`). 
> 
> In case you use a different host, port or credentials, connections settings would require adjusting.
>
> #### Where to get help
> If you're having trouble going through this tutorial you can contact us through Github issues on our
> [Steeltoe Samples Repository](https://github.com/SteeltoeOSS/Samples).


In the [previous tutorial](../Tutorial4/readme.md) we improved our
messaging flexibility. Instead of using a `fanout` exchange only capable of
dummy broadcasting, we used a `direct` one, and gained a possibility
of selectively receiving the message based on the routing key.

Although using the `direct` exchange improved our system, it still has
limitations - it can't do routing based on multiple criteria.

In our messaging system we might want to subscribe to not only queues
based on the routing key, but also based on the source which produced
the message.
You might know this concept from the
[`syslog`](http://en.wikipedia.org/wiki/Syslog) unix tool, which
routes logs based on both severity (info/warn/crit...) and facility
(auth/cron/kern...). Our example is a little simpler than this.

That example would give us a lot of flexibility - we may want to listen to
just critical errors coming from 'cron' but also all logs from 'kern'.

To implement that flexibility in our logging system we need to learn
about a more complex `topic` exchange.


Topic exchange
--------------

Messages sent to a `topic` exchange can't have an arbitrary
`routing_key` - it must be a list of words, delimited by dots. The
words can be anything, but usually they specify some features
connected to the message. A few valid routing key examples:
"`stock.usd.nyse`", "`nyse.vmw`", "`quick.orange.rabbit`". There can be as
many words in the routing key as you like, up to the limit of 255
bytes.

The routing key associated with a binding must also be in the same form. The logic behind the
`topic` exchange is similar to a `direct` one - a message sent with a
particular routing key will be delivered to all the queues that are
bound with a matching binding key. However there are two important
special cases for routing keys associated with bindings.

  * `*` (star) can substitute for exactly one word.
  * `#` (hash) can substitute for zero or more words.

It's easiest to explain this in an example:

<div class="diagram">
  <img src="../img/tutorials/python-five.png" height="170" alt="Topic Exchange illustration, which is all explained in the following text." title="Topic Exchange Illustration" />
</div>

In this example, we're going to send messages which all describe
animals. The messages will be sent with a routing key that consists of
three words (two dots). The first word in the routing key
will describe speed, second a color and third a species:
"`<speed>.<color>.<species>`".

We created three bindings: Q1 is bound with binding key "`*.orange.*`"
and Q2 with "`*.*.rabbit`" and "`lazy.#`".

These bindings can be summarized as:

  * Q1 is interested in all the orange animals.
  * Q2 wants to hear everything about rabbits, and everything about lazy
    animals.

A message with a routing key set to "`quick.orange.rabbit`"
will be delivered to both queues. Message
"`lazy.orange.elephant`" also will go to both of them. On the other hand
"`quick.orange.fox`" will only go to the first queue, and
"`lazy.brown.fox`" only to the second. "`lazy.pink.rabbit`" will
be delivered to the second queue only once, even though it matches two bindings.
"`quick.brown.fox`" doesn't match any binding so it will be discarded.

What happens if we break our contract and send a message with one or
four words, like "`orange`" or "`quick.orange.new.rabbit`"? Well,
these messages won't match any bindings and will be lost.

On the other hand "`lazy.orange.new.rabbit`", even though it has four
words, will match the last binding and will be delivered to the second
queue.

> #### Topic exchange
>
> Topic exchange is powerful and can behave like other exchanges.
>
> When a queue is bound with "`#`" (hash) binding key - it will receive
> all the messages, regardless of the routing key - like in `fanout` exchange.
>
> When special characters "`*`" (star) and "`#`" (hash) aren't used in bindings,
> the topic exchange will behave just like a `direct` one.

Putting it all together
-----------------------

We're going to use a `topic` exchange in our messaging system. We'll
start off with a working assumption that the routing keys will take
advantage of both wildcards and a hash tag.

The code is almost the same as in the
[previous tutorial](../Tutorial4/readme.md).

First let's configure all the RabbitMQ entities using the Steeltoe attributes:

```csharp
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using Steeltoe.Messaging.RabbitMQ.Config;
using System.Diagnostics;

namespace Receiver
{

    [DeclareExchange(Name = Program.TopicExchangeName, Type = ExchangeType.TOPIC)]
    [DeclareAnonymousQueue("queue1")]
    [DeclareAnonymousQueue("queue2")]
    [DeclareQueueBinding(Name = "binding.queue1.orange", ExchangeName = Program.TopicExchangeName, RoutingKey = "*.orange.*", QueueName = "#{@queue1}")]
    [DeclareQueueBinding(Name = "binding.queue1.rabbit", ExchangeName = Program.TopicExchangeName, RoutingKey = "*.*.rabbit", QueueName = "#{@queue1}")]
    [DeclareQueueBinding(Name = "binding.queue2.lazy", ExchangeName = Program.TopicExchangeName, RoutingKey = "lazy.#", QueueName = "#{@queue2}")]

    internal class Tut5Receiver
    {
        private readonly ILogger _logger;

        public Tut5Receiver(ILogger<Tut5Receiver> logger)
        {
            _logger = logger;
        }
		......
	}
}
```

The `Tut5Receiver` again uses the `RabbitListener` attribute to receive messages from the respective
topics.


```csharp
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using Steeltoe.Messaging.RabbitMQ.Config;
using System.Diagnostics;

namespace Receiver
{

    [DeclareExchange(Name = Program.TopicExchangeName, Type = ExchangeType.TOPIC)]
    [DeclareAnonymousQueue("queue1")]
    [DeclareAnonymousQueue("queue2")]
    [DeclareQueueBinding(Name = "binding.queue1.orange", ExchangeName = Program.TopicExchangeName, RoutingKey = "*.orange.*", QueueName = "#{@queue1}")]
    [DeclareQueueBinding(Name = "binding.queue1.rabbit", ExchangeName = Program.TopicExchangeName, RoutingKey = "*.*.rabbit", QueueName = "#{@queue1}")]
    [DeclareQueueBinding(Name = "binding.queue2.lazy", ExchangeName = Program.TopicExchangeName, RoutingKey = "lazy.#", QueueName = "#{@queue2}")]

    internal class Tut5Receiver
    {
        private readonly ILogger _logger;

        public Tut5Receiver(ILogger<Tut5Receiver> logger)
        {
            _logger = logger;
        }

        [RabbitListener(Queue = "#{@queue1}")]
        public void Receive1(string input)
        {
            Receive(input, 1);
        }

        [RabbitListener(Queue = "#{@queue2}")]
        public void Receive2(string input)
        {
            Receive(input, 2);
        }

        private void Receive(string input, int receiver)
        {
            var watch = new Stopwatch();
            watch.Start();

            DoWork(input);

            watch.Stop();

            var time = watch.Elapsed;
            _logger.LogInformation($"Received: {input} from queue: {receiver}, took: {time}");
        }

        private void DoWork(string input)
        {
            foreach (var ch in input)
            {
                if (ch == '.')
                    Thread.Sleep(1000);
            }
        }
    }
}
```

The code for `Tut5Sender`:

```csharp
using Steeltoe.Messaging.RabbitMQ.Core;
using System.Text;

namespace Sender
{
    public class Tut5Sender : BackgroundService
    {
        internal const string TopicExchangeName = "tut.topic";

        private readonly ILogger<Tut5Sender> _logger;
        private readonly RabbitTemplate _rabbitTemplate;

        private int index = 0;
        private int count = 0;

        private readonly string[] keys = new string[] { 
            "quick.orange.rabbit", 
            "lazy.orange.elephant", 
            "quick.orange.fox",
            "lazy.brown.fox", 
            "lazy.pink.rabbit", 
            "quick.brown.fox"};

        public Tut5Sender(ILogger<Tut5Sender> logger, RabbitTemplate rabbitTemplate)
        {
            _logger = logger;
            _rabbitTemplate = rabbitTemplate;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                StringBuilder builder = new StringBuilder("Hello to ");
                if (++index == keys.Length)
                {
                    index = 0;
                }
                string key = keys[index];
                builder.Append(key).Append(' ');
                builder.Append(++count);
                var message = builder.ToString();

                await _rabbitTemplate.ConvertAndSendAsync(TopicExchangeName, key, message);
                _logger.LogInformation($"Sent '" + message + "'");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
```

Compile as usual, see [tutorial one](../Tutorial1/readme.md)

```bash
cd tutorials\tutorial5
dotnet build
```

To run the receiver, execute the following commands:

```bash
# receiver

cd receiver
dotnet run
```

Open another shell to run the sender:

```bash
# sender

cd sender
dotnet run
```
Have fun playing with these programs. Note that the code doesn't make
any assumption about the routing or binding keys, you may want to play
with more than two routing key parameters.

Next, find out how to do a round trip message as a remote procedure call (RPC)
in [tutorial 6](../Tutorial6/readme.md)
