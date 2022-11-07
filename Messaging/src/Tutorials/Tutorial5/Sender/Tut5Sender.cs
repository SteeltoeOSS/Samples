using Steeltoe.Messaging.RabbitMQ.Core;
using System.Text;

namespace Sender
{
    public class Tut5Sender : BackgroundService
    {
        internal const string TopicExchangeName = "tut.topic";

        private readonly ILogger<Tut5Sender> _logger;
        private readonly RabbitTemplate _rabbitTemplate;

        private int index = 0;
        private int count = 0;

        private readonly string[] keys = new string[] { 
            "quick.orange.rabbit", 
            "lazy.orange.elephant", 
            "quick.orange.fox",
            "lazy.brown.fox", 
            "lazy.pink.rabbit", 
            "quick.brown.fox"};

        public Tut5Sender(ILogger<Tut5Sender> logger, RabbitTemplate rabbitTemplate)
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
                if (++index == keys.Length)
                {
                    index = 0;
                }
                string key = keys[index];
                builder.Append(key).Append(' ');
                builder.Append(++count);
                var message = builder.ToString();

                await _rabbitTemplate.ConvertAndSendAsync(TopicExchangeName, key, message);
                _logger.LogInformation($"Sent '" + message + "'");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}