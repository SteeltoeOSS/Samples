using Steeltoe.Common;
using Steeltoe.Samples.ActuatorApi.Data;

namespace Steeltoe.Samples.ActuatorApi.AdminTasks;

internal class ResetTask(WeatherDbContext weatherDbContext, ILogger<ForecastTask> logger) : IApplicationTask
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        logger.LogWarning("Removing all forecast data...");
        weatherDbContext.RemoveRange(weatherDbContext.Forecasts);
        await weatherDbContext.SaveChangesAsync(cancellationToken);
    }
}
