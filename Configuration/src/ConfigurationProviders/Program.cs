using Steeltoe.Common.Logging;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Configuration.ConfigServer;
using Steeltoe.Configuration.Kubernetes.ServiceBinding;
using Steeltoe.Configuration.Placeholder;
using Steeltoe.Configuration.RandomValue;
using Steeltoe.Management.Endpoint;
using Steeltoe.Samples.ConfigurationProviders.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Steeltoe: Add Configuration providers.
builder.Configuration.AddRandomValueSource(BootstrapLoggerFactory.Default);
builder.Configuration.AddKubernetesServiceBindings();
builder.Configuration.AddCloudFoundry(null, BootstrapLoggerFactory.Default);
builder.Configuration.AddPlaceholderResolver(BootstrapLoggerFactory.Default);
builder.AddConfigServer(BootstrapLoggerFactory.Default);

// Steeltoe: Add actuator endpoints.
builder.AddAllActuators();

// Steeltoe: map VCAP_APPLICATION and VCAP_SERVICES to IOptions<CloudFoundryApplicationOptions> and IOptions<CloudFoundryServicesOptions>
builder.Services.AddCloudFoundryOptions();

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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();
