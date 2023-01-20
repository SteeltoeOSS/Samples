using Redis;
using Steeltoe.Connector.Redis;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Steeltoe: Setup
builder.AddCloudFoundryConfiguration();
builder.AddAllActuators();

// Steeltoe: Add the Redis distributed cache.
// We are using the Steeltoe Redis Connector to pickup the CloudFoundry Redis Service binding and use it to configure
// the underlying Redis cache. This adds an IDistributedCache to the container.
builder.Services.AddDistributedRedisCache(builder.Configuration);

// Steeltoe: This works like the above, but adds an IConnectionMultiplexer to the container.
builder.Services.AddRedisConnectionMultiplexer(builder.Configuration);

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
