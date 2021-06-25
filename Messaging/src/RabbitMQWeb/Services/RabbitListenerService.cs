using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace RabbitMQWeb.Services
{
    /// <summary>
    /// A service class that will handle incoming messages 
    /// </summary>
    public class RabbitListenerService
    {
        public const string INFERRED_FOO_QUEUE = "sample1.inferred.foo";
        public const string INFERRED_BAR_QUEUE = "sample1.inferred.bar";

        private ILogger _logger;
        public RabbitListenerService(ILogger<RabbitListenerService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Process any messages put on the INFERRED_FOO_QUEUE
        /// </summary>
        /// <param name="foo"></param>
        [RabbitListener(INFERRED_FOO_QUEUE)]
        public void ListenForAFoo(Foo foo)
        {
            _logger.LogInformation("Expected a Foo, got a " + foo);
        }

        /// <summary>
        /// Process any messages put on the INFERRED_BAR_QUEUE 
        /// </summary>
        /// <param name="bar"></param>
        [RabbitListener(INFERRED_BAR_QUEUE)]
        public void ListenForAFoo(Bar bar)
        {
            _logger.LogInformation("Expected a Bar, got a " + bar);
        }
    }
}
