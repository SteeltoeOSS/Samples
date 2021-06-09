using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Messaging.Support;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsageSender
{
    [EnableBinding(typeof(ISource))]
    public class UsageGenerator : BackgroundService
    {
        private readonly ISource _source;
        private readonly ILogger<UsageGenerator> _logger;
        private static readonly Random RANDOM = new Random();
   
        public string ServiceName { get; set; } = "UsageGenerator";
        private string[] users = { "user1", "user2", "user3", "user4", "user5" };

        public UsageGenerator(ISource source, ILogger<UsageGenerator> logger)
        {
            _source = source;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(5000, stoppingToken); // Wait for the Infrastructure to be setup correctly; 
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    var message = GenerateAndSend();
                    _source.Output.Send(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
                await Task.Delay(5000, stoppingToken);
            }
        }

        [SendTo(ISource.OUTPUT)]
        protected virtual IMessage GenerateAndSend()
        {
            var value = new UsageDetail
            {
                UserId = users[RANDOM.Next(5)],
                Duration = RANDOM.Next(300),
                Data = RANDOM.Next(700)
            };

            _logger.LogInformation("Sending: " + value);
            return MessageBuilder.WithPayload(value).Build();
        }
    }
}