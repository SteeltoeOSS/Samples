// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Steeltoe.Common;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Connectors.EntityFrameworkCore;
using Steeltoe.Connectors.EntityFrameworkCore.MySql;
using Steeltoe.Connectors.MySql;
using Steeltoe.Management.Endpoint;
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

if (builder.Configuration.GetValue<bool>("UseSpringBootAdmin"))
{
    builder.Services.AddSpringBootAdminClient();
    builder.Services.SecureActuatorsWithBasicAuth();
}
else
{
    builder.AddCloudFoundryConfiguration();
}

// Steeltoe: Add actuator endpoints, with an authorization policy outside of Cloud Foundry
builder.AddAllActuators(configureEndpoints =>
{
    if (builder.Configuration.GetValue<bool>("UseSpringBootAdmin"))
    {
        configureEndpoints.RequireAuthorization("actuators.read");
    }
});
builder.Services.AddPrometheusActuator();

builder.Services.ConfigureOpenTelemetry(builder.Configuration);

// Steeltoe: Setup MySQL options, connection factory and health checks.
builder.AddMySql();

// Steeltoe: Add Entity Framework db context, bound to connection string in configuration
builder.Services.AddDbContext<WeatherContext>((serviceProvider, options) => options.UseMySql(serviceProvider));

// Steeltoe: Register tasks for managing DbContext migrations and data
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

// Causes certificate trust issues with SBA, port issues with ManagementPortMiddleware
//app.UseHttpsRedirection();

app.MapGet("/WeatherForecast", ([FromServices]WeatherContext context, [FromServices]ILoggerFactory loggerFactory, [FromQuery]string? fromDate, [FromQuery]int daysCount = 5) =>
{
    var _logger = loggerFactory.CreateLogger<WeatherForecast>();
    var queryDate = string.IsNullOrEmpty(fromDate) ? DateTime.Now : DateTime.Parse(fromDate);
    _logger.LogInformation("Determining the {daysCount}-day forecast starting from {ForecastQueryDate}.", daysCount, DateOnly.FromDateTime(queryDate));
    var forecast = context.Forecasts.Where(f => f.Date >= DateOnly.FromDateTime(queryDate) && f.Date < DateOnly.FromDateTime(queryDate.AddDays(daysCount)));
    if (forecast.Count() < daysCount)
    {
        // TODO: provide the exact command to fix the data shortage
        _logger.LogError("Relevant forecast data for only {forecastCount} day(s) was found. Use the forecast task to generate the missing data.", forecast.Count());
        if (Platform.IsCloudFoundry)
        {
            _logger.LogInformation("cf run-task ./??? runtask=Forecast --fromDate={fromDate} --days={daysCount}", fromDate, daysCount);
        }
        else
        {
            _logger.LogInformation("dotnet run --runtask=Forecast --fromDate={fromDate} --days={daysCount}", fromDate, daysCount);
        }
    }

    // sleep a random amount of milliseconds for variance in trace data
    Thread.Sleep(Random.Shared.Next(10, 3000));

    _logger.LogCritical("Test Critical message");
    _logger.LogError("Test Error message");
    _logger.LogWarning("Test Warning message");
    _logger.LogInformation("Test Informational message");
    _logger.LogDebug("Test Debug message");
    _logger.LogTrace("Test Trace message");

    return forecast;
}).WithName("GetWeatherForecast").WithOpenApi().AllowAnonymous();
app.MapGet("/AllForecastData", ([FromServices]WeatherContext context) => context.Forecasts).WithName("GetAllForecastData").WithOpenApi().AllowAnonymous();

// Steeltoe: Insert some rows into MySQL table.
await MySqlSeeder.CreateSampleDataAsync(app.Services);

await app.RunWithTasksAsync(default);
