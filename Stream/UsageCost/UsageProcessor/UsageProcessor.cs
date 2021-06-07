using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using System;

namespace UsageProcessor
{
    [EnableBinding(typeof(IProcessor))]
    public class UsageProcessor
    {
        private static ILogger<UsageProcessor> _logger;

        private double _ratePerSecond = 0.1;

        private double _ratePerMB = 0.05;

        public UsageProcessor(ILogger<UsageProcessor> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [StreamListener(IProcessor.INPUT)]
        [SendTo(IProcessor.OUTPUT)]
        public UsageCostDetail Handle(UsageDetail usageDetail)
        {
            var costDetail = new UsageCostDetail
            {
                UserId = usageDetail.UserId,
                CallCost = usageDetail.Duration * _ratePerSecond,
                DataCost = usageDetail.Data * _ratePerMB
            };
            _logger.LogInformation("Processed UsageCostDetail " + costDetail);

            return costDetail;
        }
    }
}
