using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint.Actuators.All;
using Steeltoe.Management.Endpoint.Actuators.Health;
using Steeltoe.Samples.FileSharesWeb;

var builder = WebApplication.CreateBuilder(args);

// Steeltoe: Add Cloud Foundry Configuration Provider to read service bindings.
builder.AddCloudFoundryConfiguration();

builder.Services.AddHealthContributor<FileShareHealthContributor>();
// Steeltoe: Add actuator endpoints.
builder.Services.AddAllActuators();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
