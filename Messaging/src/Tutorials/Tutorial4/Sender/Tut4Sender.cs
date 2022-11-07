using Steeltoe.Messaging.RabbitMQ.Core;
using System.Text;

namespace Sender
{
    public class Tut4Sender : BackgroundService
    {
        internal const string DirectExchangeName = "tut.direct";

        private readonly ILogger<Tut4Sender> _logger;
        private readonly RabbitTemplate _rabbitTemplate;

        private int index = 0;
        private int count = 0;

        private readonly string[] keys = new string[] { "orange", "black", "green" };

        public Tut4Sender(ILogger<Tut4Sender> logger, RabbitTemplate rabbitTemplate)
        {
            _logger = logger;
            _rabbitTemplate = rabbitTemplate;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                StringBuilder builder = new StringBuilder("Hello to ");
                if (++index == 3)
                {
                    index = 0;
                }
                string key = keys[index];
                builder.Append(key).Append(' ');
                builder.Append(++count);
                var message = builder.ToString();

                await _rabbitTemplate.ConvertAndSendAsync(DirectExchangeName, key, message);
                _logger.LogInformation($"Sent '" + message + "'");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}