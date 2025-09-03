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
