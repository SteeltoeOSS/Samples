using Steeltoe.Messaging;
using Steeltoe.Messaging.RabbitMQ.Connection;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using static Steeltoe.Messaging.RabbitMQ.Core.RabbitTemplate;

namespace Sender
{
    public class Tut7Sender : BackgroundService, IReturnCallback, IConfirmCallback
    {
        private readonly ILogger<Tut7Sender> _logger;
        private readonly RabbitTemplate _rabbitTemplate;
        private int id;

        public Tut7Sender(ILogger<Tut7Sender> logger, IServiceProvider provider)
        {
            _logger = logger;
            _rabbitTemplate = provider.GetRabbitTemplate("confirmReturnsTemplate");
            _rabbitTemplate.ReturnCallback = this;
            _rabbitTemplate.ConfirmCallback = this;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                CorrelationData data = new CorrelationData(id.ToString());
                id++;
                await _rabbitTemplate.ConvertAndSendAsync(Program.QueueName, (object)"Hello World!", data, stoppingToken);
                _logger.LogInformation("Worker running at: {time}, sent ID: {id}", DateTimeOffset.Now, data.Id);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public void ReturnedMessage(IMessage<byte[]> message, int replyCode, string replyText, string exchange, string routingKey)
        {
            _logger.LogInformation($"Returned message: ReplyCode={replyCode}, Exchange={exchange}, RoutingKey={routingKey}");
        }

        public void Confirm(CorrelationData correlationData, bool ack, string cause)
        {
            _logger.LogInformation($"Confirming message: Id={correlationData.Id}, Acked={ack}, Cause={cause}");
        }
    }
}