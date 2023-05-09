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
