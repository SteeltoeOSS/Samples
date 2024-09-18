// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Steeltoe.Common;
using Steeltoe.Connectors.EntityFrameworkCore;
using Steeltoe.Connectors.EntityFrameworkCore.MySql;
using Steeltoe.Connectors.MySql;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.SpringBootAdminClient;
using Steeltoe.Management.Tasks;
using Steeltoe.Samples.ActuatorApi;
using Steeltoe.Samples.ActuatorApi.Data;
using Steeltoe.Samples.ActuatorApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Steeltoe: Add actuator endpoints, with an authorization policy outside of Cloud Foundry
builder.AddAllActuators(configureEndpoints =>
{
    if (!Platform.IsCloudFoundry)
    {
        configureEndpoints.RequireAuthorization("actuators.read");
    }
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Steeltoe: Setup MySQL options, connection factory and health checks.
builder.AddMySql();

// Steeltoe: Add Entity Framework db context, bound to connection string in configuration
builder.Services.AddDbContext<WeatherContext>((serviceProvider, options) => options.UseMySql(serviceProvider));

// Steeltoe: Register a task for applying code-first DbContext migrations
builder.Services.AddTask<MigrateDbContextTask<WeatherContext>>("Database-Migration");
builder.Services.AddTask<ForecastTask>("forecast");

if (!Platform.IsCloudFoundry)
{
    builder.Services.AddSpringBootAdminClient();
    //builder.Services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
    //           .AddBasic(basicOptions =>
    //           {
    //               //"/actuator", new Claim("scope", "actuators.read")
    //           });
    //builder.Services.AddAuthorizationBuilder()
    //    .AddPolicy("actuators.read", policy => policy.RequireClaim("scope", "actuators.read"));
}

// Add OpenTelemetry
builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder.AddAspNetCoreInstrumentation();
        tracerProviderBuilder.AddOtlpExporter(otlpExporterOptions =>
        {
            // otlpExporterOptions.Endpoint = new Uri("<OTLP Export Address>");
        });
        tracerProviderBuilder.AddZipkinExporter(zipkinExporterOptions =>
        {
            zipkinExporterOptions.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
        });
    });
builder.Services.ConfigureOpenTelemetryTracerProvider((serviceProvider, tracerProviderBuilder) =>
{
    var appName = serviceProvider.GetRequiredService<IApplicationInstanceInfo>().ApplicationName ?? "ActuatorApi-fallbackName";
    tracerProviderBuilder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(appName));
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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
