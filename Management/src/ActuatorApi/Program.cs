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

// Steeltoe: Register with Spring Boot Admin.
if (builder.Configuration.GetValue<bool>("UseSpringBootAdmin"))
{
    builder.Services.AddSpringBootAdminClient();
}

builder.Services.ConfigureOpenTelemetry(builder.Configuration);

// Steeltoe: Setup MySQL options, connection factory and health checks.
builder.AddMySql();

// Steeltoe: Add Entity Framework db context, bound to connection string in configuration.
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

// Steeltoe: The next line is commented out because Actuators are listening on a dedicated port, which does not support https.
// app.UseHttpsRedirection();

// Steeltoe: Map weather-related endpoints.
WeatherEndpoints.Map(app);

// Steeltoe: Insert some rows into MySQL table when not running as a task.
if (!app.HasApplicationTask())
{
    await MySqlSeeder.CreateSampleDataAsync(app.Services);
}

await app.RunWithTasksAsync(default);
