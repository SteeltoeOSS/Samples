using Steeltoe.Common;
using Steeltoe.Samples.ActuatorApi.Data;

namespace Steeltoe.Samples.ActuatorApi.AdminTasks;

internal class ResetTask(WeatherDbContext weatherContext, ILogger<ForecastTask> logger) : IApplicationTask
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        logger.LogWarning("Removing all forecast data...");
        weatherContext.RemoveRange(weatherContext.Forecasts);
        await weatherContext.SaveChangesAsync(cancellationToken);
    }
}
