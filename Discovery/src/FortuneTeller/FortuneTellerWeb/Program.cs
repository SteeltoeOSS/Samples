using FortuneTellerWeb.Services;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Steeltoe.Common;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Configuration.CloudFoundry.ServiceBinding;
using Steeltoe.Discovery.Configuration;
using Steeltoe.Discovery.Consul;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Discovery.HttpClients;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Steeltoe: Read credentials to Eureka server from VCAP_SERVICES on CloudFoundry.
builder.AddCloudFoundryConfiguration();
builder.Configuration.AddCloudFoundryServiceBindings();

// Steeltoe: Add service discovery clients for Consul, Eureka, and configuration-based.
builder.Services.AddConsulDiscoveryClient();
builder.Services.AddEurekaDiscoveryClient();
builder.Services.AddConfigurationDiscoveryClient();

// Steeltoe: Register HTTP client to use service discovery.
builder.Services.AddHttpClient<FortuneService>(httpClient => httpClient.BaseAddress = new Uri("https://fortuneService/api/fortunes/")).AddServiceDiscovery();

// Add OpenTelemetry
builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder.AddHttpClientInstrumentation();
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
    var appName = serviceProvider.GetRequiredService<IApplicationInstanceInfo>()
        .GetApplicationNameInContext(SteeltoeComponent.Management);
    tracerProviderBuilder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(appName));
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();
