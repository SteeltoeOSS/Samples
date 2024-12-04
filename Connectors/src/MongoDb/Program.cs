using MongoDB.Driver;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Connectors.MongoDb;
using Steeltoe.Management.Endpoint.Actuators.All;
using Steeltoe.Samples.MongoDb;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Steeltoe: Add Cloud Foundry Configuration Provider for Actuator integration (not required for connectors).
builder.AddCloudFoundryConfiguration();

// Steeltoe: Add actuator endpoints.
builder.Services.AddAllActuators();

// Steeltoe: Setup MongoDB options, connection factory and health checks.
builder.AddMongoDb();

// Steeltoe: optionally change the MongoDB connection URL at runtime.
builder.Services.Configure<MongoDbOptions>(options =>
{
    var urlBuilder = new MongoUrlBuilder(options.ConnectionString)
    {
        ApplicationName = "mongodb-connector-sample"
    };

    options.ConnectionString = urlBuilder.ToString();
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

// Steeltoe: Insert some objects into MongoDB collection.
await MongoDbSeeder.CreateSampleDataAsync(app.Services);

app.Run();
