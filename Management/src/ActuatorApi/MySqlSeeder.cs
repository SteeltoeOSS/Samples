using Steeltoe.Samples.ActuatorApi.Data;
using Steeltoe.Samples.ActuatorApi.Models;

namespace Steeltoe.Samples.ActuatorApi;

internal static class MySqlSeeder
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

    public static async Task CreateSampleDataAsync(IServiceProvider serviceProvider)
    {
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
        await using var appDbContext = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();

        await ForecastTheNextWeekAsync(appDbContext);
    }

    /// <summary>
    /// Predict the weather for the 7 days after the last date that was previously forecast.
    /// </summary>
    private static async Task ForecastTheNextWeekAsync(WeatherDbContext dbContext)
    {
        DateOnly firstNewForecastDate = DateOnly.FromDateTime(DateTime.Now);

        try
        {
            firstNewForecastDate = dbContext.Forecasts.Max(wf => wf.Date).AddDays(1);
        }
        catch (InvalidOperationException)
        {
            // 'Sequence contains no elements.' is expected when there is no existing forecast data.
        }

        for (int index = 0; index < 7; index++)
        {
            DateOnly dateToForecast = firstNewForecastDate.AddDays(index);
            dbContext.Forecasts.Add(MakeForecast(dateToForecast));
        }

        await dbContext.SaveChangesAsync();
    }

    internal static WeatherForecast MakeForecast(DateOnly date)
    {
        return new WeatherForecast(date, Random.Shared.Next(-20, 55), Summaries[Random.Shared.Next(Summaries.Length)]);
    }
}
