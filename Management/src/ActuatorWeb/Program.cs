using Steeltoe.Common;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Actuators.All;
using Steeltoe.Management.Endpoint.SpringBootAdminClient;
using Steeltoe.Management.Prometheus;
using Steeltoe.Management.Tracing;
using Steeltoe.Samples.ActuatorWeb;
using Steeltoe.Samples.ActuatorWeb.CustomActuators.LocalTime;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

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
    builder.Services.AddPrometheusActuator(true, branchedApplicationBuilder =>
    {
        branchedApplicationBuilder.UseAuthorization();
    });
}
else
{
    // Steeltoe: Map Steeltoe-configured OpenTelemetry Prometheus exporter.
    builder.Services.AddPrometheusActuator();
}

// Steeltoe: Add custom actuator that displays the local server time.
// See appsettings.Development.json for how the response format can be configured.
builder.Services.AddLocalTimeActuator();

// Steeltoe: Register with Spring Boot Admin.
if (builder.Configuration.GetValue<bool>("UseSpringBootAdmin"))
{
    builder.Services.AddSpringBootAdminClient();
}

// Steeltoe: Ensure log entries include "[app-name, traceId, spanId]" for log correlation.
builder.Services.AddTracingLogProcessor();

// Steeltoe: Configure OpenTelemetry app instrumentation and export of metrics and traces.
builder.Services.ConfigureOpenTelemetry(builder.Configuration);

// Steeltoe: Register typed HttpClient for communicating with the backend service.
builder.Services.AddHttpClient<ActuatorApiClient>(SetBaseAddress);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Steeltoe: The next line is commented out because Actuators are listening on a dedicated port, which does not support https.
// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

// This code is used to limit complexity in the sample. A real application should use Service Discovery.
// To learn more about service discovery, review the documentation: https://docs.steeltoe.io/api/v3/discovery/
static void SetBaseAddress(IServiceProvider serviceProvider, HttpClient client)
{
    var instanceInfo = serviceProvider.GetRequiredService<IApplicationInstanceInfo>();

    if (instanceInfo is CloudFoundryApplicationOptions { Uris.Count: > 0 } options)
    {
        string address = options.Uris.First();
        string baseAddress = address.Replace("actuator-web", "actuator-api");
        client.BaseAddress = new Uri($"https://{baseAddress}");
    }
    else
    {
        client.BaseAddress = new Uri("http://localhost:5140");
    }

    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    ILogger<Program> logger = loggerFactory.CreateLogger<Program>();

    logger.LogInformation("HttpClient BaseAddress set to {BaseAddress}", client.BaseAddress);
}
