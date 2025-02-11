using Microsoft.EntityFrameworkCore;

namespace Steeltoe.Samples.ActuatorApi.Models;

[PrimaryKey(nameof(Date))]
public sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
