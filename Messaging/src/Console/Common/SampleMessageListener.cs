using Microsoft.Extensions.Logging;
using Steeltoe.Messaging;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Listener;
using System.Text;

namespace Console.Common;

public class SampleMessageListener(ILogger<SampleMessageListener> logger) : IMessageListener
{
    public AcknowledgeMode ContainerAckMode { get; set; }

    public void OnMessage(IMessage message)
    {
        var payload = Encoding.UTF8.GetString((byte[])message.Payload);
        logger.LogInformation("Received message: {payload}", payload);
    }

    public void OnMessageBatch(List<IMessage> messages)
    {
        foreach (var message in messages)
        {
            OnMessage(message);
        }
    }
}