using Common;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace MonitorRabbitMQ;

public class RabbitListenerService(ILogger<RabbitListenerService> logger)
{
    private const string ReceiveAndConvertQueue = "steeltoe_message_queue";

    [RabbitListener(ReceiveAndConvertQueue)]
    public void ListenForAMessage(Message msg)
    {
        logger.LogInformation("Received the message '{Message}' from the queue.", msg);
    }
}