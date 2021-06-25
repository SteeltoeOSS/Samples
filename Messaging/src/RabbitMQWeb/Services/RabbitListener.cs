using Microsoft.Extensions.Logging;
using RabbitMQWeb.Models;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace RabbitMQWeb.Services
{
    public class RabbitListener
    {
        private ILogger _logger;

        public RabbitListener(ILogger<RabbitListener> logger)
        {
            _logger = logger;
        }

        [RabbitListener(Queues.InferredRabbitQueue)]
        public void ListenForAFoo(RabbitMessage message)
        {
            _logger.LogInformation("Got a RabbitMessage: {Message}", message);
        }

        [RabbitListener(Queues.InferredLongEaredRabbitQueue)]
        public void ListenForAFoo(LongEaredRabbitMessage message)
        {
            _logger.LogInformation("Got a LongEaredRabbitMessage: {Message}", message);
        }
    }
}