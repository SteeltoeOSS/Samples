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
        public void ListenForAFoo(RabbitMessage rabbitMessage)
        {
            _logger.LogInformation("Expected a Foo, got a {Message}", rabbitMessage);
        }

        [RabbitListener(Queues.InferredLongEaredRabbitQueue)]
        public void ListenForAFoo(LongEaredRabbitMessage longEaredRabbitMessage)
        {
            _logger.LogInformation("Expected a Bar, got a {Message}", longEaredRabbitMessage);
        }
    }
}