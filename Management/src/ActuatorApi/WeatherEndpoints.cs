using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Steeltoe.Common;
using Steeltoe.Samples.ActuatorApi.Data;
using Steeltoe.Samples.ActuatorApi.Models;

namespace Steeltoe.Samples.ActuatorApi;

internal static class WeatherEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/WeatherForecast", async (WeatherDbContext dbContext, TimeProvider timeProvider, ILoggerFactory loggerFactory, [FromQuery] string? fromDate,
            [FromQuery] int days = 5, CancellationToken cancellationToken = default) =>
        {
            // Steeltoe: Log messages at various levels for loggers actuator demonstration.
            ILogger logger = loggerFactory.CreateLogger(typeof(WeatherEndpoints));
            logger.LogCritical("Test Critical message");
            logger.LogError("Test Error message");
            logger.LogWarning("Test Warning message");
            logger.LogInformation("Test Informational message");
            logger.LogDebug("Test Debug message");
            logger.LogTrace("Test Trace message");

            DateOnly startDate = string.IsNullOrEmpty(fromDate)
                ? DateOnly.FromDateTime(timeProvider.GetLocalNow().Date)
                : DateOnly.Parse(fromDate, CultureInfo.InvariantCulture);

            logger.LogInformation("Retrieving the {DayCount}-day forecast starting from {ForecastQueryDate}.", days, startDate);
            List<WeatherForecast> forecasts = await GetForecastsAsync(dbContext, startDate, days, cancellationToken);

            if (forecasts.Count < days)
            {
                if (Platform.IsCloudFoundry)
                {
                    logger.LogWarning(
                        "Relevant forecast data was found for only {DayCountFound} day(s). Use the ForecastWeather application task to generate the missing data:{LineBreak}" +
                        "cf run-task actuator-api-management-sample --command \"./Steeltoe.Samples.ActuatorApi runtask=ForecastWeather --fromDate={FromDateParameter} --days={DayCountParameter}\"",
                        forecasts.Count, Environment.NewLine, startDate.ToString("yyyy-MM-dd"), days);
                }
                else
                {
                    logger.LogWarning(
                        "Relevant forecast data was found for only {DayCountFound} day(s). Use the ForecastWeather application task to generate the missing data:{LineBreak}" +
                        "dotnet run --runtask=ForecastWeather --fromDate={FromDateParameter} --days={DayCountParameter}", forecasts.Count, Environment.NewLine,
                        startDate.ToString("yyyy-MM-dd"), days);
                }
            }

            // Steeltoe: Sleep a random amount of milliseconds for variance in trace data.
            await Task.Delay(TimeSpan.FromMilliseconds(Random.Shared.Next(10, 3000)), timeProvider, cancellationToken);

            return forecasts;
        }).WithName("GetWeatherForecast").AllowAnonymous();

        app.MapGet("/AllForecastData",
            async (WeatherDbContext dbContext, CancellationToken cancellationToken = default) =>
                await GetForecastsAsync(dbContext, null, -1, cancellationToken)).WithName("GetAllForecastData").AllowAnonymous();
    }

    private static async Task<List<WeatherForecast>> GetForecastsAsync(WeatherDbContext dbContext, DateOnly? startDate, int days,
        CancellationToken cancellationToken)
    {
        try
        {
            IQueryable<WeatherForecast> query = dbContext.Forecasts;

            if (startDate != null && days > -1)
            {
                query = query.Where(forecast => forecast.Date >= startDate.Value && forecast.Date < startDate.Value.AddDays(days));
            }

            return await query.ToListAsync(cancellationToken);
        }
        catch (MySqlException exception)
        {
            throw new InvalidOperationException("Failed to access the database. Please follow the steps in README.md to create and seed the database.",
                exception);
        }
    }
}
