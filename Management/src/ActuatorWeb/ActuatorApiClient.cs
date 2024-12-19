using Microsoft.AspNetCore.Http.Extensions;
using Steeltoe.Samples.ActuatorWeb.Pages;

namespace Steeltoe.Samples.ActuatorWeb;

public sealed class ActuatorApiClient(HttpClient httpClient)
{
    public async Task<List<WeatherForecast>> GetWeatherForecastsAsync(string? fromDate, int? days, CancellationToken cancellationToken)
    {
        string requestUri = BuildWeatherForecastRequestUri(fromDate, days);
        HttpResponseMessage response = await httpClient.GetAsync(requestUri, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<WeatherForecast>>(cancellationToken) ?? [];
        }

        string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        string errorMessage = $"Failed to retrieve weather forecasts. Server response HTTP {(int)response.StatusCode}:{Environment.NewLine}{responseBody}";
        throw new HttpRequestException(errorMessage);
    }

    private static string BuildWeatherForecastRequestUri(string? fromDate, int? days)
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

        return $"weatherForecast{builder.ToQueryString()}";
    }
}
