using Microsoft.Extensions.Logging;
using RabbitMQWeb.Models;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace RabbitMQWeb.Services;

public class RabbitListener(ILogger<RabbitListener> logger)
{
    private readonly ILogger _logger = logger;

    [RabbitListener(Queues.InferredRabbitQueue)]
    public void ListenForMessage(RabbitMessage message)
    {
        _logger.LogInformation("Got a RabbitMessage: {Message}", message);
    }

    [RabbitListener(Queues.InferredLongEaredRabbitQueue)]
    public void ListenForMessage(LongEaredRabbitMessage message)
    {
        _logger.LogInformation("Got a LongEaredRabbitMessage: {Message}", message);
    }
}