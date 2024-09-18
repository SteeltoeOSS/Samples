using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Steeltoe.Connectors.Redis;
using Steeltoe.Management.Endpoint.Actuators.All;
using Steeltoe.Samples.Redis;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Steeltoe: Add actuator endpoints.
builder.Services.AddAllActuators();

// Steeltoe: Setup Redis options, connection factory and health checks, optionally providing a callback to customize client settings.
builder.AddRedis(null, addOptions =>
{
    // Optionally provide a callback to customize client settings.
    addOptions.CreateConnection = (serviceProvider, serviceBindingName) =>
    {
        var optionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<RedisOptions>>();
        RedisOptions options = optionsMonitor.Get(serviceBindingName);

        ConfigurationOptions redisOptions = !string.IsNullOrWhiteSpace(options.ConnectionString)
            ? ConfigurationOptions.Parse(options.ConnectionString)
            : new ConfigurationOptions();

        redisOptions.ClientName = "redis-connector-sample";
        return ConnectionMultiplexer.Connect(redisOptions);
    };
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

// Steeltoe: Add some key/value pairs to the Redis cache.
await RedisSeeder.CreateSampleDataAsync(app.Services);

app.Run();
