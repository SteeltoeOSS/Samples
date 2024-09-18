// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ActuatorWeb.Pages;

public class WeatherModel(IHttpClientFactory httpClientFactory) : PageModel
{
    public required WeatherForecast[] Forecasts { get; set; } = [];

    public async Task OnGet()
    {
        using var httpClient = httpClientFactory.CreateClient(nameof(WeatherModel));
        try
        {
            var response = await httpClient.GetAsync("weatherforecast");
            Forecasts = await response.Content.ReadFromJsonAsync<WeatherForecast[]>() ?? [ new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), 0, "Failed to read response.") ];
        }
        catch (Exception exception) when (exception is HttpRequestException)
        {
            Forecasts = [ new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), 0, $"Encountered {exception.GetType().Name} connecting to {httpClient.BaseAddress}. Confirm the API is running/deployed.") ];
        }
    }
}

public sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
