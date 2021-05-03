using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging;
using System;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;

namespace PolledConsumer
{
    public class Worker : BackgroundService, IMessageHandler
    {
        private readonly IPolledConsumerBinding _binding;
        private readonly ILogger<Worker> _logger;

        public string ServiceName { get; set; } = "BackgroundWorker";

        public Worker(IPolledConsumerBinding binding, ILogger<Worker> logger)
        {
            _binding = binding;
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
                    if (!_binding.DestIn.Poll(this))
                    {
                        await Task.Delay(2000, stoppingToken);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }
            }
        }

        public void HandleMessage(IMessage message)
        {
            try
            {
                var payloadString = (string)message.Payload;
                var newPayload = payloadString.ToUpper();
                _logger.LogInformation("Received Message : " + payloadString);
                _binding.DestOut.Send(Message.Create(newPayload));
                _logger.LogInformation("Sent Message : " + newPayload);

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}