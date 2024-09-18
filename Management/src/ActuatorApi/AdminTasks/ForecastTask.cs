using Steeltoe.Common;
using Steeltoe.Samples.ActuatorApi.Data;

namespace Steeltoe.Samples.ActuatorApi.AdminTasks;

public class ForecastTask(WeatherContext weatherContext, ILogger<ForecastTask> logger) : IApplicationTask
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        var args = Environment.GetCommandLineArgs();
        var fromDateString = args.FirstOrDefault(text => text.StartsWith("--fromDate", StringComparison.OrdinalIgnoreCase))?.Split("=")[1];
        var fromDate = DateOnly.FromDateTime(DateTime.Parse(fromDateString ?? DateTime.Now.ToString()));
        var days = int.Parse(args.FirstOrDefault(text => text.StartsWith("--days", StringComparison.OrdinalIgnoreCase))?.Split("=")[1] ?? "7");

        for (var index = 0; index < days; index++)
        {
            var dateToForecast = fromDate.AddDays(index);
            if (!weatherContext.Forecasts.Any(weather => weather.Date == dateToForecast))
            {
                weatherContext.Forecasts.Add(MySqlSeeder.MakeForecast(dateToForecast));
            }
            else
            {
                logger.LogWarning("The weather for {date} has already been forecast!", dateToForecast);
            }
        }

        await weatherContext.SaveChangesAsync(cancellationToken);
    }
}
