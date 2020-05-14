using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.Rabbit.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQWeb.Services
{
    public class RabbitListenerService
    {
        public const string INFERRED_FOO_QUEUE = "sample1.inferred.foo";
        public const string INFERRED_BAR_QUEUE = "sample1.inferred.bar";

        private ILogger _logger;
        public RabbitListenerService(ILogger<RabbitListenerService> logger)
        {
            _logger = logger;
        }

        [RabbitListener(INFERRED_FOO_QUEUE)]
        public void ListenForAFoo(Foo foo)
        {
            _logger.LogInformation("Expected a Foo, got a " + foo);
        }

        [RabbitListener(INFERRED_BAR_QUEUE)]
        public void ListenForAFoo(Bar bar)
        {
            _logger.LogInformation("Expected a Bar, got a " + bar);
        }
    }
}
