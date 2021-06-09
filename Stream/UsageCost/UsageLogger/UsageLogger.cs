using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using System;

namespace UsageLogger
{
    [EnableBinding(typeof(ISink))]
    public class UsageLogger
    {
        private static ILogger<UsageLogger> _logger;

        public UsageLogger(ILogger<UsageLogger> logger)
        {
            _logger = logger ?? NullLogger<UsageLogger>.Instance;
        }

        [StreamListener(IProcessor.INPUT)]
        public void Handle(UsageCostDetail costDetail) =>
            _logger.LogInformation("Received UsageCostDetail " + costDetail);

    }
}
