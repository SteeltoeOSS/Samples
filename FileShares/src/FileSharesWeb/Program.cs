using Microsoft.Extensions.DependencyInjection.Extensions;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint.Actuators.All;
using Steeltoe.Management.Endpoint.Actuators.Health.Contributors;
using Steeltoe.Samples.FileSharesWeb;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Steeltoe: Add Cloud Foundry Configuration Provider to read service bindings.
builder.AddCloudFoundryConfiguration();

// Steeltoe: Add a class to hold the file share configuration.
builder.Services.AddSingleton<FileShareConfiguration>();

// Steeltoe: Add a hosted service for managing the file share.
builder.Services.AddHostedService<FileShareHostedService>();

// Steeltoe: Add actuator endpoints.
builder.Services.AddAllActuators();

// Steeltoe: Configure the disk space health contributor to monitor the file share.
builder.Services.AddOptions<DiskSpaceContributorOptions>()
    .PostConfigure<FileShareConfiguration>((diskSpaceOptions, fileShareConfiguration) => diskSpaceOptions.Path = fileShareConfiguration.Location);

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
