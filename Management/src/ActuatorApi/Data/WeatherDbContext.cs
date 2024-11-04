using Microsoft.EntityFrameworkCore;
using Steeltoe.Samples.ActuatorApi.Models;

namespace Steeltoe.Samples.ActuatorApi.Data;

public sealed class WeatherDbContext(DbContextOptions<WeatherDbContext> options)
    : DbContext(options)
{
    public DbSet<WeatherForecast> Forecasts { get; set; }
}
