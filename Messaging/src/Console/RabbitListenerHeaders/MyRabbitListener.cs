using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace RabbitListenerHeaders;

public class MyRabbitListener(ILogger<MyRabbitListener> logger)
{
    [RabbitListener("myQueue")]
    public void Listen(Order input, [Header("order_type")] string orderType)
    {
        logger.LogInformation("Order received: {input}", input);
        logger.LogInformation("Header={fromHeader}", orderType);
    }
}