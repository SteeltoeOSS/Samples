using Microsoft.AspNetCore.Mvc;
using Steeltoe.Common;
using Steeltoe.Samples.ActuatorApi.Data;
using Steeltoe.Samples.ActuatorApi.Models;

namespace Steeltoe.Samples.ActuatorApi;

internal static class WeatherEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/WeatherForecast", async (WeatherDbContext context, ILoggerFactory loggerFactory, [FromQuery] string? fromDate, [FromQuery] int days = 5) =>
        {
            // Steeltoe: Log messages at various levels for loggers actuator demonstration.
            ILogger<WeatherForecast> logger = loggerFactory.CreateLogger<WeatherForecast>();
            logger.LogCritical("Test Critical message");
            logger.LogError("Test Error message");
            logger.LogWarning("Test Warning message");
            logger.LogInformation("Test Informational message");
            logger.LogDebug("Test Debug message");
            logger.LogTrace("Test Trace message");

            DateTime queryDate = string.IsNullOrEmpty(fromDate) ? DateTime.Now : DateTime.Parse(fromDate);

            logger.LogInformation("Determining the {DayCount}-day forecast starting from {ForecastQueryDate}.", days, DateOnly.FromDateTime(queryDate));

            IQueryable<WeatherForecast> forecast = context.Forecasts.Where(f =>
                f.Date >= DateOnly.FromDateTime(queryDate) && f.Date < DateOnly.FromDateTime(queryDate.AddDays(days)));

            if (forecast.Count() < days)
            {
                logger.LogError("Relevant forecast data was found for only {DayCount} day(s). Use the forecast task to generate the missing data.",
                    forecast.Count());

                if (Platform.IsCloudFoundry)
                {
                    logger.LogInformation(
                        "cf run-task actuator-api-management-sample --command \"./Steeltoe.Samples.ActuatorApi runtask=Forecast --fromDate={FromDate} --days={DayCount}\"",
                        queryDate.ToString("yyyy-MM-dd"), days);
                }
                else
                {
                    logger.LogInformation("dotnet run --runtask=Forecast --fromDate={FromDate} --days={DayCount}", queryDate.ToString("yyyy-MM-dd"), days);
                }
            }

            // Steeltoe: Sleep a random amount of milliseconds for variance in trace data.
            await Task.Delay(Random.Shared.Next(10, 3000));

            return forecast;
        }).WithName("GetWeatherForecast").WithOpenApi().AllowAnonymous();

        app.MapGet("/AllForecastData", (WeatherDbContext context) => context.Forecasts).WithName("GetAllForecastData").WithOpenApi().AllowAnonymous();
    }
}
