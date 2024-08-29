using Microsoft.AspNetCore.DataProtection;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Connectors.Redis;
using Steeltoe.Security.DataProtection.Redis;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddCloudFoundryConfiguration();

// Steeltoe: Setup Redis connector.
builder.AddRedis();

// Steeltoe: Enable session state.
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Steeltoe: Setup data protection to use Redis connector.
builder.Services.AddDataProtection().PersistKeysToRedis().SetApplicationName("redis-data-protection-sample");

// Add services to the container.
builder.Services.AddRazorPages();

WebApplication app = builder.Build();

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

// Steeltoe: Activate session state.
app.UseSession();

app.MapRazorPages();

app.Run();
