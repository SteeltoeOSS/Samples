using Steeltoe.Common.Logging;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Configuration.ConfigServer;
using Steeltoe.Configuration.Kubernetes.ServiceBindings;
using Steeltoe.Configuration.Placeholder;
using Steeltoe.Configuration.RandomValue;
using Steeltoe.Management.Endpoint.Actuators.All;
using Steeltoe.Samples.ConfigurationProviders.Models;

// Steeltoe: Log to the console until the app has fully started.
var bootstrapLoggerFactory = BootstrapLoggerFactory.CreateConsole(loggingBuilder =>
{
    loggingBuilder.AddConfiguration(new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
    {
        ["LogLevel:Steeltoe.Configuration"] = "Trace"
    }).Build());
});

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Steeltoe: Add Configuration providers.
builder.Configuration.AddRandomValueSource(bootstrapLoggerFactory);
builder.Configuration.AddKubernetesServiceBindings();
builder.Configuration.AddPlaceholderResolver(bootstrapLoggerFactory);
builder.AddCloudFoundryConfiguration(bootstrapLoggerFactory);
builder.AddConfigServer(bootstrapLoggerFactory);

// Steeltoe: Upgrade the created bootstrap loggers from settings in the service container.
builder.Services.UpgradeBootstrapLoggerFactory(bootstrapLoggerFactory);

// Steeltoe: Add actuator endpoints.
builder.Services.AddAllActuators();

// Steeltoe: Optionally enables usage of "spring:cloud:config" keys to configure Spring Cloud Config Server.
builder.Services.ConfigureConfigServerClientOptions();

// Steeltoe: Add the configuration data POCO configured with data returned from the Spring Cloud Config Server.
builder.Services.Configure<ExternalConfiguration>(builder.Configuration);
builder.Services.Configure<PlaceholderValues>(builder.Configuration);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();

app.Run();
