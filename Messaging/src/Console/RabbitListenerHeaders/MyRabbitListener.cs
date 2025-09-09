using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace RabbitListenerHeaders;

public class MyRabbitListener
{
    private readonly ILogger<MyRabbitListener> _logger;

    public MyRabbitListener(ILogger<MyRabbitListener> logger)
    {
        _logger = logger;
    }

    [RabbitListener("myQueue")]
    public void Listen(Order input, [Header("order_type")] string orderType)
    {
        _logger.LogInformation("Order received: {input}", input);
        _logger.LogInformation("Header={fromHeader}", orderType);
    }
}