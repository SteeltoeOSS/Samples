using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace RabbitListener;

public class MyRabbitListener
{
    private readonly ILogger<MyRabbitListener> _logger;

    public MyRabbitListener(ILogger<MyRabbitListener> logger)
    {
        _logger = logger;
    }

    [RabbitListener("myQueue")]
    public void Listen(string input)
    {
        _logger.LogInformation("Received message: {input}", input);
    }
}