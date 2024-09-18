using Steeltoe.Common;
using Steeltoe.Samples.ActuatorApi.Data;

namespace Steeltoe.Samples.ActuatorApi.AdminTasks;

public class ResetTask(WeatherContext weatherContext, ILogger<ForecastTask> logger) : IApplicationTask
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        logger.LogWarning("Removing all forecast data...");
        weatherContext.RemoveRange(weatherContext.Forecasts);
        await weatherContext.SaveChangesAsync(cancellationToken);
    }
}
