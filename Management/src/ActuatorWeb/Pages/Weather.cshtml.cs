using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Steeltoe.Samples.ActuatorWeb.Pages;

public class WeatherModel(IHttpClientFactory httpClientFactory, ILogger<WeatherModel> logger) : PageModel
{
    internal string? ForecastStartDate { get; set; }
    internal string DaysSelected { get; set; } = string.Empty;
    internal WeatherForecast[] Forecasts { get; set; } = [];
    internal Exception? RequestException { get; set; }

    public async Task OnGet(string? fromDate, int? days)
    {
        // Use these log entries to demonstrate changing log levels with the loggers actuator.
        logger.LogCritical("Test Critical message");
        logger.LogError("Test Error message");
        logger.LogWarning("Test Warning message");
        logger.LogInformation("Test Informational message");
        logger.LogDebug("Test Debug message");
        logger.LogTrace("Test Trace message");

        using HttpClient httpClient = httpClientFactory.CreateClient(nameof(WeatherModel));

        try
        {
            string requestUri = BuildRequestUri(fromDate, days);

            HttpResponseMessage response = await httpClient.GetAsync(requestUri);

            Forecasts = await response.Content.ReadFromJsonAsync<WeatherForecast[]>() ?? [];
        }
        catch (Exception exception) when (exception is HttpRequestException)
        {
            RequestException = exception;
        }

        ForecastStartDate = fromDate ?? DateTime.Now.ToString("yyyy-MM-dd");
        DaysSelected = days?.ToString() ?? string.Empty;
    }

    private static string BuildRequestUri(string? fromDate, int? days)
    {
        var builder = new QueryBuilder();

        if (fromDate != null)
        {
            builder.Add("fromDate", fromDate);
        }

        if (days != null)
        {
            builder.Add("days", days.Value.ToString());
        }

        return "weatherForecast" + builder.ToQueryString();
    }
}

public sealed record WeatherForecast(DateOnly Date, int TemperatureC, int TemperatureF, string? Summary);
