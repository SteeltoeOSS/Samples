using Common;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace MonitorRabbitMQ
{
    public class RabbitListenerService
    {
        public const string RECEIVE_AND_CONVERT_QUEUE = "steeltoe_message_queue";
        private ILogger _logger;

        public RabbitListenerService(ILogger<RabbitListenerService> logger)
        {
            _logger = logger;
        }

        [RabbitListener(RECEIVE_AND_CONVERT_QUEUE)]
        public void ListenForAMessage(Message msg)
        {
            _logger.LogInformation($"Received the message '{msg}' from the queue.");
        }
    }
}
