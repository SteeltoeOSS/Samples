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
