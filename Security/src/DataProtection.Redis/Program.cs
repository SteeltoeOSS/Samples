using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
//using Steeltoe.Configuration.CloudFoundry;
//using Steeltoe.Connectors.Redis;
using Steeltoe.Connector.Redis;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint;
using Steeltoe.Security.DataProtection;
using Steeltoe.Security.DataProtection.Redis;


var builder = WebApplication.CreateBuilder(args);

builder.AddCloudFoundryConfiguration();

// Steeltoe: Setup Redis connector.
//builder.AddRedis(null, addOptions =>
//{
    //// Optionally provide a callback to customize client settings.
    //addOptions.CreateConnection = (serviceProvider, serviceBindingName) =>
    //{
    //    var optionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<RedisOptions>>();
    //    RedisOptions options = optionsMonitor.Get(serviceBindingName);
        
    //    ConfigurationOptions redisOptions = !string.IsNullOrWhiteSpace(options.ConnectionString)
    //        ? ConfigurationOptions.Parse(options.ConnectionString)
    //        : new ConfigurationOptions();
    //    redisOptions.ServiceName
    //    redisOptions.ClientName = "redis-connector";
    //    return ConnectionMultiplexer.Connect(redisOptions);
    //};
//});

builder.Services.AddRedisConnectionMultiplexer(builder.Configuration);
builder.Services.AddDistributedRedisCache(builder.Configuration);

// Steeltoe: Enable session state.
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Steeltoe: Setup data protection to use Redis connector.
builder.Services.AddDataProtection().PersistKeysToRedis().SetApplicationName("steeltoe-samples-dataprotection-redis");

// Add services to the container.
builder.Services.AddRazorPages();

// Steeltoe: Add actuators
builder.AddAllActuators();

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

// Steeltoe: Activate session state.
app.UseSession();

app.MapRazorPages();

app.Run();
