using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace RabbitListener;

public class MyRabbitListener(ILogger<MyRabbitListener> logger)
{
    [RabbitListener("myQueue")]
    public void Listen(string input)
    {
        logger.LogInformation("Received message: {input}", input);
    }
}