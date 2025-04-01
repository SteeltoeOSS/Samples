using System.Net;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint.Actuators.All;
using Steeltoe.Management.Endpoint.Actuators.Health;
using Steeltoe.Samples.FileSharesWeb;
using Steeltoe.Samples.FileSharesWeb.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Steeltoe: Add Cloud Foundry Configuration Provider to read service bindings.
builder.AddCloudFoundryConfiguration();

// Steeltoe: Add health contributor for network file share.
builder.Services.AddHealthContributor<FileShareHealthContributor>();

// Steeltoe: Add actuator endpoints.
builder.Services.AddAllActuators();

// Steeltoe: Add options for holding network file share location and credentials.
builder.Services.AddOptions<FileShareOptions>().PostConfigure<IOptions<CloudFoundryServicesOptions>>((shareOptions, serviceOptions) =>
{
    if (!serviceOptions.Value.Services.TryGetValue("credhub", out IList<CloudFoundryService>? value))
    {
        throw new InvalidOperationException();
    }

    CloudFoundryService? credHubEntry = value.FirstOrDefault(service => service.Name!.Equals("sampleNetworkShare"));
    shareOptions.Location = credHubEntry?.Credentials["location"].Value ?? throw new InvalidOperationException("Network share path is required.");

    string userName = credHubEntry.Credentials["username"].Value ?? throw new InvalidOperationException("Network share username is required.");
    string password = credHubEntry.Credentials["password"].Value ?? throw new InvalidOperationException("Network share password is required.");
    shareOptions.Credential = new NetworkCredential(userName, password);
});

// Steeltoe: Add a time provider for use when naming file uploads.
builder.Services.TryAddSingleton(TimeProvider.System);

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
