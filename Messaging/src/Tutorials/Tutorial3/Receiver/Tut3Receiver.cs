using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using Steeltoe.Messaging.RabbitMQ.Config;
using System.Diagnostics;

namespace Receiver
{
    [DeclareExchange(Name = "tut.fanout", Type = ExchangeType.FANOUT)]
    [DeclareAnonymousQueue("queue1")]
    [DeclareAnonymousQueue("queue2")]
    [DeclareQueueBinding(Name = "tut.fanout.binding.queue1", ExchangeName = "tut.fanout", QueueName = "#{@queue1}")]
    [DeclareQueueBinding(Name = "tut.fanout.binding.queue2", ExchangeName = "tut.fanout", QueueName = "#{@queue2}")]
    internal class Tut3Receiver
    {
        private readonly ILogger _logger;

        public Tut3Receiver(ILogger<Tut3Receiver> logger)
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
