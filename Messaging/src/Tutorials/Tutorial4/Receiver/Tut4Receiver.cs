using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using Steeltoe.Messaging.RabbitMQ.Config;
using System.Diagnostics;

namespace Receiver
{
    [DeclareExchange(Name = "tut.direct", Type = ExchangeType.DIRECT)]
    [DeclareAnonymousQueue("queue1")]
    [DeclareAnonymousQueue("queue2")]
    [DeclareQueueBinding(Name = "tut.direct.binding.queue1.orange", ExchangeName = "tut.direct", RoutingKey = "orange", QueueName = "#{@queue1}")]
    [DeclareQueueBinding(Name = "tut.direct.binding.queue1.black", ExchangeName = "tut.direct", RoutingKey = "black", QueueName = "#{@queue1}")]
    [DeclareQueueBinding(Name = "tut.direct.binding.queue2.green", ExchangeName = "tut.direct", RoutingKey = "green", QueueName = "#{@queue2}")]
    [DeclareQueueBinding(Name = "tut.direct.binding.queue2.black", ExchangeName = "tut.direct", RoutingKey = "black", QueueName = "#{@queue2}")]
    internal class Tut4Receiver
    {
        private readonly ILogger _logger;

        public Tut4Receiver(ILogger<Tut4Receiver> logger)
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
