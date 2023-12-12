using Microsoft.AspNetCore.DataProtection;
using Steeltoe.Connectors.Redis;
using Steeltoe.Security.DataProtection.Redis;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Steeltoe: Setup Redis connector.
builder.AddRedis();

// Steeltoe: Setup data protection to use Redis connector.
builder.Services.AddDataProtection().PersistKeysToRedis().SetApplicationName("redis-keystore");

// Steeltoe: Enable session state.
builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();

WebApplication app = builder.Build();

// Steeltoe: Activate session state.
app.UseSession();

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
