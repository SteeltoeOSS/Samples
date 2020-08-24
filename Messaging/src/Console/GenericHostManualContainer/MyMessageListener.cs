using Microsoft.Extensions.Logging;
using Steeltoe.Messaging;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Listener;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGenericHost
{
    public class MyMessageListener : IMessageListener
    {
        private ILogger<MyMessageListener> logger;

        public MyMessageListener(ILogger<MyMessageListener> logger)
        {
            this.logger = logger;
        }

        public AcknowledgeMode ContainerAckMode { get; set; }

        public void OnMessage(IMessage message)
        {
            var payload = Encoding.UTF8.GetString((byte[])message.Payload);
            logger.LogInformation(payload);
        }

        public void OnMessageBatch(List<IMessage> messages)
        {
            foreach(var message in messages)
            {
                OnMessage(message);
            }
        }
    }
}
