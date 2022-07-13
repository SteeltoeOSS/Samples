using Common.SeedWork;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace Notification.API
{
    public class ListenerService
    {
        private ILogger _logger;

        public ListenerService(ILogger<ListenerService> logger)
        {
            _logger = logger;
        }

        [RabbitListener(Queues.ProductAddQueue)]
        public void ListenForAMessage(Message msg)
        {
            _logger.LogInformation($"Received the message '{msg}' from the queue.");
        }
    }
}
