using Common;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace MonitorRabbitMQ;

public class RabbitListenerService
{
    private readonly ILogger<RabbitListenerService> _logger;

    public RabbitListenerService(ILogger<RabbitListenerService> logger)
    {
        _logger = logger;
    }

    [RabbitListener(Constants.ReceiveAndConvertQueue)]
    public void ListenForAMessage(Message msg)
    {
        _logger.LogInformation("Received the message '{Message}' from the queue.", msg);
    }
}