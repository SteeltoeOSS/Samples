using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Steeltoe.Samples.ActuatorWeb.Pages;

public class WeatherModel(ActuatorApiClient actuatorApiClient, ILogger<WeatherModel> logger) : PageModel
{
    internal string? ForecastStartDate { get; set; }
    internal string DaysSelected { get; set; } = string.Empty;
    internal List<WeatherForecast> Forecasts { get; set; } = [];
    internal Exception? RequestException { get; set; }

    public async Task OnGet(string? fromDate, int? days, CancellationToken cancellationToken)
    {
        // Use these log entries to demonstrate changing log levels with the loggers actuator.
        logger.LogCritical("Test Critical message");
        logger.LogError("Test Error message");
        logger.LogWarning("Test Warning message");
        logger.LogInformation("Test Informational message");
        logger.LogDebug("Test Debug message");
        logger.LogTrace("Test Trace message");

        try
        {
            Forecasts = await actuatorApiClient.GetWeatherForecastsAsync(fromDate, days, cancellationToken);
        }
        catch (Exception exception) when (exception is HttpRequestException)
        {
            RequestException = exception;
        }

        ForecastStartDate = fromDate ?? DateTime.Now.ToString("yyyy-MM-dd");
        DaysSelected = days?.ToString() ?? string.Empty;
    }
}

public sealed record WeatherForecast(DateOnly Date, int TemperatureC, int TemperatureF, string? Summary);
