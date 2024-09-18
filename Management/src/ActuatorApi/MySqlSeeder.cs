// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Infrastructure;
// using Microsoft.EntityFrameworkCore.Storage;
using Steeltoe.Samples.ActuatorApi.Data;
using Steeltoe.Samples.ActuatorApi.Models;

namespace Steeltoe.Samples.ActuatorApi;

internal sealed class MySqlSeeder
{
    private static readonly string[] summaries =
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
        // Exit early if command-line args are present to avoid conflict with Steeltoe.Samples.ActuatorApi.ForecastTask
        var args = Environment.GetCommandLineArgs();
        if (args.Length != 0)
        {
            return;
        }

        await using var scope = serviceProvider.CreateAsyncScope();
        await using var appDbContext = scope.ServiceProvider.GetRequiredService<WeatherContext>();

        // Code-first schema management retained here for potential use in the future
        // await CreateTablesAsync(appDbContext);
        await ForecastTheNextWeekAsync(appDbContext);
    }

    //private static async Task CreateTablesAsync(DbContext dbContext)
    //{
        //var wasCreated = await dbContext.Database.EnsureCreatedAsync();
        //if (wasCreated)
        //{
        //    // The database already existed. Because apps usually don't have permission to drop the database,
        //    // we drop and recreate all the tables in the DbContext instead.
        //    var databaseCreator = (RelationalDatabaseCreator)dbContext.Database.GetService<IDatabaseCreator>();
        //    await databaseCreator.CreateTablesAsync();
        //}
    //}

    /// <summary>
    /// Predict the weather for the 7 days after the last date that was previously forecast
    /// </summary>
    private static async Task ForecastTheNextWeekAsync(WeatherContext dbContext)
    {
        var firstNewForecastDate = DateOnly.FromDateTime(DateTime.Now);
        try
        {
            firstNewForecastDate = dbContext.Forecasts.Max(wf => wf.Date).AddDays(1);
        }
        catch
        {
            // At startup, throws 'System.InvalidOperationException: Sequence contains no elements.'
            // TODO: determine if there is anything else we want to handle
        }

        for (var index = 0; index < 7; index++)
        {
            var dateToForecast = firstNewForecastDate.AddDays(index);
            dbContext.Forecasts.Add(MakeForecast(dateToForecast));
        }

        await dbContext.SaveChangesAsync();
    }

    internal static WeatherForecast MakeForecast(DateOnly date)
    {
        return new WeatherForecast(date, Random.Shared.Next(-20, 55), summaries[Random.Shared.Next(summaries.Length)]);
    }
}
