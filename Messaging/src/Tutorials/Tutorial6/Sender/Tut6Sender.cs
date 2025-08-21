using Steeltoe.Messaging.RabbitMQ.Core;

namespace Sender
{
    public class Tut6Sender : BackgroundService
    {
        internal const string RPCExchangeName = "tut.rpc";
        private readonly ILogger<Tut6Sender> _logger;
        private readonly RabbitTemplate _rabbitTemplate;
        private int start = 0;

        public Tut6Sender(ILogger<Tut6Sender> logger, RabbitTemplate rabbitTemplate)
        {
            _logger = logger;
            _rabbitTemplate = rabbitTemplate;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _logger.LogInformation($"Requesting Fib({start})");
                int result = await _rabbitTemplate.ConvertSendAndReceiveAsync<int>(RPCExchangeName, "rpc", start++, stoppingToken);
                _logger.LogInformation($"Got result: {result}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}