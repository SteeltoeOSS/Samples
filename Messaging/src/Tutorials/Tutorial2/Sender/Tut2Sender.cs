using Steeltoe.Messaging.RabbitMQ.Core;
using System.Text;

namespace Sender
{
    public class Tut2Sender : BackgroundService
    {
        private const string QueueName = "hello";

        private readonly ILogger<Tut2Sender> _logger;
        private readonly RabbitTemplate _rabbitTemplate;
        private int dots = 0;
        private int count = 0;


        public Tut2Sender(ILogger<Tut2Sender> logger, RabbitTemplate rabbitTemplate)
        {
            _logger = logger;
            _rabbitTemplate = rabbitTemplate;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var message = CreateMessage();
                await _rabbitTemplate.ConvertAndSendAsync(QueueName, message);
                _logger.LogInformation($"Sent '" + message + "'");
                await Task.Delay(1000, stoppingToken);
            }
        }

        private string CreateMessage()
        {
            StringBuilder builder = new StringBuilder("Hello");
            if (++dots == 4)
            {
                dots = 1;
            }
            for (int i = 0; i < dots; i++)
            {
                builder.Append('.');
            }
            builder.Append(++count);
            return builder.ToString();
        }
    }
}