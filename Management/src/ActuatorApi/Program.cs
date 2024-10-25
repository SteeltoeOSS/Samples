using Microsoft.AspNetCore.Mvc;
using Steeltoe.Common;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Connectors.EntityFrameworkCore;
using Steeltoe.Connectors.EntityFrameworkCore.MySql;
using Steeltoe.Connectors.MySql;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Actuators.All;
using Steeltoe.Management.Endpoint.SpringBootAdminClient;
using Steeltoe.Management.Prometheus;
using Steeltoe.Management.Tasks;
using Steeltoe.Samples.ActuatorApi;
using Steeltoe.Samples.ActuatorApi.AdminTasks;
using Steeltoe.Samples.ActuatorApi.Data;
using Steeltoe.Samples.ActuatorApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Steeltoe: Add Cloud Foundry configuration provider and options classes.
builder.AddCloudFoundryConfiguration();

// Steeltoe: Add all actuators, specifying an authorization policy for use outside the Cloud Foundry context.
builder.Services.ConfigureActuatorEndpoints(configureEndpoints =>
{
    if (!Platform.IsCloudFoundry)
    {
        configureEndpoints.RequireAuthorization("actuator.read");
    }
});
builder.Services.ConfigureActuatorAuth();
builder.Services.AddAllActuators();
builder.Services.AddPrometheusActuator();
if (builder.Configuration.GetValue<bool>("UseSpringBootAdmin"))
{
    builder.Services.AddSpringBootAdminClient();
}

builder.Services.ConfigureOpenTelemetry(builder.Configuration);

// Steeltoe: Setup MySQL options, connection factory and health checks.
builder.AddMySql();

// Steeltoe: Add Entity Framework db context, bound to connection string in configuration.
builder.Services.AddDbContext<WeatherContext>((serviceProvider, options) => options.UseMySql(serviceProvider));

// Steeltoe: Register tasks for managing DbContext migrations and data.
builder.Services.AddTask<MigrateDbContextTask<WeatherContext>>("MigrateDatabase");
builder.Services.AddTask<ForecastTask>("ForecastWeather");
builder.Services.AddTask<ResetTask>("ResetWeather");

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/WeatherForecast", async ([FromServices] WeatherContext context, [FromServices] ILoggerFactory loggerFactory, [FromQuery] string? fromDate, [FromQuery] int daysCount = 5) =>
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
    logger.LogInformation("Determining the {daysCount}-day forecast starting from {ForecastQueryDate}.", daysCount, DateOnly.FromDateTime(queryDate));
    IQueryable<WeatherForecast>? forecast = context.Forecasts.Where(f => f.Date >= DateOnly.FromDateTime(queryDate) && f.Date < DateOnly.FromDateTime(queryDate.AddDays(daysCount)));
    if (forecast.Count() < daysCount)
    {
        logger.LogError("Relevant forecast data was found for only {DayCount} day(s) was found. Use the forecast task to generate the missing data.", forecast.Count());
        if (Platform.IsCloudFoundry)
        {
            logger.LogInformation("cf run-task actuator-api-management-sample --command \"./Steeltoe.Samples.ActuatorApi runtask=Forecast --fromDate={fromDate} --days={daysCount}\"", queryDate, daysCount);
        }
        else
        {
            logger.LogInformation("dotnet run --runtask=Forecast --fromDate={fromDate} --days={daysCount}", queryDate, daysCount);
        }
    }

    // Steeltoe: Sleep a random amount of milliseconds for variance in trace data.
    await Task.Delay(Random.Shared.Next(10, 3000));

    return forecast;
}).WithName("GetWeatherForecast").WithOpenApi().AllowAnonymous();
app.MapGet("/AllForecastData", ([FromServices] WeatherContext context) => context.Forecasts).WithName("GetAllForecastData").WithOpenApi().AllowAnonymous();

// Steeltoe: Insert some rows into MySQL table.
await MySqlSeeder.CreateSampleDataAsync(app.Services);

await app.RunWithTasksAsync(default);
