using Steeltoe.Samples.FortuneTellerConsole.Models;
using Steeltoe.Samples.FortuneTellerConsole.Services;

namespace Steeltoe.Samples.FortuneTellerConsole;

public sealed class Worker(FortuneService fortuneService) : BackgroundService
{
    private readonly FortuneService _fortuneService = fortuneService;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine($"Worker running at: {DateTimeOffset.Now} (press Ctrl+C to close).");

            FortuneModel? fortune = await _fortuneService.GetRandomFortuneAsync(stoppingToken);
            Console.WriteLine($"Fortune: {fortune}.");

            await Task.Delay(1000, stoppingToken);
        }
    }
}
