using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace RabbitMQWeb.Services
{
    /// <summary>
    /// A service class that will handle incoming messages 
    /// </summary>
    public class RabbitListenerService
    {

        private ILogger _logger;
        public RabbitListenerService(ILogger<RabbitListenerService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Process any messages put on the INFERRED_FOO_QUEUE
        /// </summary>
        /// <param name="foo"></param>
        [RabbitListener(Queues.InferredFooQueue)]
        public void ListenForAFoo(Foo foo)
        {
            _logger.LogInformation("Expected a Foo, got a {Message}", foo);
        }

        /// <summary>
        /// Process any messages put on the INFERRED_BAR_QUEUE 
        /// </summary>
        /// <param name="bar"></param>
        [RabbitListener(Queues.InferredBarQueue)]
        public void ListenForAFoo(Bar bar)
        {
            _logger.LogInformation("Expected a Bar, got a {Message}", bar);
        }
    }
}
