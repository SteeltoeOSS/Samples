using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Messaging.RabbitMQ.Attributes;

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
