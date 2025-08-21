using Steeltoe.Messaging.RabbitMQ.Core;

namespace Sender
{
    public class Tut1Sender : BackgroundService
    {
        private readonly ILogger<Tut1Sender> _logger;
        private readonly RabbitTemplate _rabbitTemplate;

        public Tut1Sender(ILogger<Tut1Sender> logger, RabbitTemplate rabbitTemplate)
        {
            _logger = logger;
            _rabbitTemplate = rabbitTemplate;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _rabbitTemplate.ConvertAndSendAsync(Program.QueueName, "Hello World!", stoppingToken);
                _logger.LogInformation("Worker running at: {time}, sent message!", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}