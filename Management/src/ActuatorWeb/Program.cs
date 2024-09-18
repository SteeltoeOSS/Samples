using ActuatorWeb.Pages;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Steeltoe.Common;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.SpringBootAdminClient;

var builder = WebApplication.CreateBuilder(args);

// Steeltoe: Add actuator endpoints, with an authorization policy outside of Cloud Foundry
builder.AddAllActuators(configureEndpoints =>
{
    if (!Platform.IsCloudFoundry)
    {
        configureEndpoints.RequireAuthorization("actuators.read");
    }
});

// Add services to the container.
builder.Services.AddRazorPages();

// Steeltoe: Register HttpClients for communicating with a backend service, including an application instance certificate for authorization.
builder.Services.AddHttpClient<WeatherModel>(SetBaseAddress);

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
    var appInfo = serviceProvider.GetRequiredService<IApplicationInstanceInfo>();
    tracerProviderBuilder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(appInfo.ApplicationName ?? "ActuatorWeb-fallback"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
return;

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
}
