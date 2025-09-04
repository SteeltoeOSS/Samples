using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging;
using Steeltoe.Messaging.Support;
using Steeltoe.Stream.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PartitionedProducer;

public class Worker : BackgroundService
{
    private readonly ISource _source;
    private readonly ILogger<Worker> _logger;
    private static readonly Random RANDOM = new Random();
    private static readonly string[] data = new string[] {
        "abc1", "def1", "qux1",
        "abc2", "def2", "qux2",
        "abc3", "def3", "qux3",
        "abc4", "def4", "qux4",
    };
    public string ServiceName { get; set; } = "BackgroundWorker";

    public Worker(ISource source, ILogger<Worker> logger)
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
                var message = Generate();
                _source.Output.Send(message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            await Task.Delay(5000, stoppingToken);
        }
    }

    protected virtual IMessage Generate()
    {
        var value = data[RANDOM.Next(data.Length)];
        Console.WriteLine("Sending: " + value);
        return MessageBuilder.WithPayload(value).SetHeader("partitionKey", value).Build();
    }
}