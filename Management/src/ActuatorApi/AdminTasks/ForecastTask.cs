using System.Globalization;
using Steeltoe.Common;
using Steeltoe.Samples.ActuatorApi.Data;

namespace Steeltoe.Samples.ActuatorApi.AdminTasks;

internal class ForecastTask(WeatherDbContext weatherDbContext, ILogger<ForecastTask> logger) : IApplicationTask
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        string[] args = Environment.GetCommandLineArgs();
        string? fromDateString = args.FirstOrDefault(text => text.StartsWith("--fromDate", StringComparison.OrdinalIgnoreCase))?.Split("=")[1];
        DateOnly fromDate = DateOnly.FromDateTime(fromDateString == null ? DateTime.Now : DateTime.Parse(fromDateString, CultureInfo.InvariantCulture));
        int days = int.Parse(args.FirstOrDefault(text => text.StartsWith("--days", StringComparison.OrdinalIgnoreCase))?.Split("=")[1] ?? "7");

        for (int index = 0; index < days; index++)
        {
            DateOnly dateToForecast = fromDate.AddDays(index);

            if (!weatherDbContext.Forecasts.Any(weather => weather.Date == dateToForecast))
            {
                weatherDbContext.Forecasts.Add(MySqlSeeder.MakeForecast(dateToForecast));
            }
            else
            {
                logger.LogWarning("The weather for {DateToForecast} has already been forecast!", dateToForecast);
            }
        }

        await weatherDbContext.SaveChangesAsync(cancellationToken);
    }
}
