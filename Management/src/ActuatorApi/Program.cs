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
using Steeltoe.Management.Tracing;
using Steeltoe.Samples.ActuatorApi;
using Steeltoe.Samples.ActuatorApi.AdminTasks;
using Steeltoe.Samples.ActuatorApi.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Steeltoe: Add Cloud Foundry configuration provider and options classes.
builder.AddCloudFoundryConfiguration();

// Steeltoe: Add all actuators.
builder.Services.AddAllActuators();

// Steeltoe: Add Prometheus and specify an authorization policy for use outside Cloud Foundry.
if (!Platform.IsCloudFoundry)
{
    builder.Services.ConfigureActuatorAuth();

    builder.Services.ConfigureActuatorEndpoints(configureEndpoints =>
    {
        configureEndpoints.RequireAuthorization("actuator.read");
    });

    // Steeltoe: Map Steeltoe-configured OpenTelemetry Prometheus exporter with authorization middleware in the pipeline.
    builder.Services.AddPrometheusActuator(true, configurePrometheusPipeline =>
    {
        configurePrometheusPipeline.UseAuthorization();
    });
}
else
{
    // Steeltoe: Map Steeltoe-configured OpenTelemetry Prometheus exporter.
    builder.Services.AddPrometheusActuator();
}

// Steeltoe: Register with Spring Boot Admin.
if (builder.Configuration.GetValue<bool>("UseSpringBootAdmin"))
{
    builder.Services.AddSpringBootAdminClient();
}

// Steeltoe: Ensure log entries include "[app-name, traceId, spanId]" for log correlation.
builder.Services.AddTracingLogProcessor();

// Steeltoe: Configure OpenTelemetry app instrumentation and export of metrics and traces.
builder.Services.ConfigureOpenTelemetry(builder.Configuration);

// Steeltoe: Setup MySQL options, connection factory and health checks.
builder.AddMySql();

// Steeltoe: Add Entity Framework Core DbContext, bound to connection string in configuration.
builder.Services.AddDbContext<WeatherDbContext>((serviceProvider, options) => options.UseMySql(serviceProvider));

// Steeltoe: Register tasks for managing DbContext migrations and data.
builder.Services.AddTask<MigrateDbContextTask<WeatherDbContext>>("MigrateDatabase");
builder.Services.AddTask<ForecastTask>("ForecastWeather");
builder.Services.AddTask<ResetTask>("ResetWeather");

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Steeltoe: See the note in the ActuatorWeb README under "Regarding HTTPS and Basic Authentication"
// app.UseHttpsRedirection();

// Steeltoe: Map weather-related endpoints.
WeatherEndpoints.Map(app);

await app.RunWithTasksAsync(default);
