using Redis;
using StackExchange.Redis;
using Steeltoe.Configuration.CloudFoundry.ServiceBinding;
using Steeltoe.Connector.Redis;
using Steeltoe.Management.Endpoint;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Steeltoe: Add cloud service bindings.
builder.Configuration.AddCloudFoundryServiceBindings();

// Steeltoe: Add actuator endpoints.
builder.AddAllActuators();

// Steeltoe: Setup Redis options, connection factory and health checks, optionally providing a callback to customize client settings.
builder.AddRedis((options, _) =>
{
    ConfigurationOptions redisOptions = ConfigurationOptions.Parse(options.ConnectionString);
    redisOptions.ClientName = "redis-connector";
    return ConnectionMultiplexer.Connect(redisOptions);
});

// Add services to the container.
builder.Services.AddControllersWithViews();

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
