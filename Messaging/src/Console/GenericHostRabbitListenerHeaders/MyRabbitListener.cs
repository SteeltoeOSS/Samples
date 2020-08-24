using Microsoft.Extensions.Logging;
using Steeltoe.Messaging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Messaging.RabbitMQ;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using Steeltoe.Messaging.Support;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGenericHost
{
    public class MyRabbitListener
    {
        private ILogger<MyRabbitListener> logger;

        public MyRabbitListener(ILogger<MyRabbitListener> logger)
        {
            this.logger = logger;
        }

        [RabbitListener("myqueue")]
        public void Listen(Order input, [Header("order_type")] string orderType)
        {
            logger.LogInformation(input.ToString());
            logger.LogInformation("Header=" + orderType);
        }
    }
}
