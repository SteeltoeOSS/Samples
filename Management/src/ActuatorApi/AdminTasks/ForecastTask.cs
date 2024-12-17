using System.Globalization;
using Steeltoe.Common;
using Steeltoe.Samples.ActuatorApi.Data;
using Steeltoe.Samples.ActuatorApi.Models;

namespace Steeltoe.Samples.ActuatorApi.AdminTasks;

internal class ForecastTask(WeatherDbContext weatherDbContext, ILogger<ForecastTask> logger) : IApplicationTask
{
    private static readonly string[] Summaries =
    [
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    ];

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
                weatherDbContext.Forecasts.Add(MakeForecast(dateToForecast));
            }
            else
            {
                logger.LogWarning("The weather for {DateToForecast} has already been forecast!", dateToForecast);
            }
        }

        await weatherDbContext.SaveChangesAsync(cancellationToken);
    }

    private static WeatherForecast MakeForecast(DateOnly date)
    {
        return new WeatherForecast(date, Random.Shared.Next(-20, 55), Summaries[Random.Shared.Next(Summaries.Length)]);
    }
}
