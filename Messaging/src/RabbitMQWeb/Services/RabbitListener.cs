using Microsoft.Extensions.Logging;
using RabbitMQWeb.Models;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace RabbitMQWeb.Services
{
    /// <summary>
    /// A service class that will handle incoming messages 
    /// </summary>
    public class RabbitListener
    {

        private ILogger _logger;
        public RabbitListener(ILogger<RabbitListener> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Process any messages put on the INFERRED_FOO_QUEUE
        /// </summary>
        /// <param name="rabbitMessage"></param>
        [RabbitListener(Queues.InferredRabbitQueue)]
        public void ListenForAFoo(RabbitMessage rabbitMessage)
        {
            _logger.LogInformation("Expected a Foo, got a {Message}", rabbitMessage);
        }

        /// <summary>
        /// Process any messages put on the INFERRED_BAR_QUEUE 
        /// </summary>
        /// <param name="longEaredRabbitMessage"></param>
        [RabbitListener(Queues.InferredLongEaredRabbitQueue)]
        public void ListenForAFoo(LongEaredRabbitMessage longEaredRabbitMessage)
        {
            _logger.LogInformation("Expected a Bar, got a {Message}", longEaredRabbitMessage);
        }
    }
}
