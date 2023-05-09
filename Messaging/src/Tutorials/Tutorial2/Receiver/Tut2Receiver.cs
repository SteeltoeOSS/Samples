using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using System.Diagnostics;

namespace Receiver
{
    [DeclareQueue(Name = "hello")]
    internal class Tut2Receiver
    {
        private readonly ILogger _logger;

        public Tut2Receiver(ILogger<Tut2Receiver> logger)
        {
            _logger = logger;
        }

        [RabbitListener(Queue = "#{@hello}")]
        public void Receive(string input)
        {
            var watch = new Stopwatch();
            watch.Start();

            DoWork(input);

            watch.Stop();

            var time = watch.Elapsed;
            _logger.LogInformation($"Received: {input} took: {time}");
        }

        private void DoWork(string input)
        {
            foreach(var ch in input)
            {
                if (ch == '.')
                    Thread.Sleep(1000);
            }
        }
    }
}
