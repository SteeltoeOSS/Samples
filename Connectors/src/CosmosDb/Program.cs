using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Options;
using Steeltoe.Connectors.CosmosDb;
using Steeltoe.Management.Endpoint.Actuators.All;
using Steeltoe.Samples.CosmosDb;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Steeltoe: Add actuator endpoints.
builder.Services.AddAllActuators();

// Steeltoe: Setup CosmosDB options, connection factory and health checks.
builder.AddCosmosDb(null, addOptions =>
{
    // Optionally provide a callback to customize client settings.
    addOptions.CreateConnection = (serviceProvider, serviceBindingName) =>
    {
        var optionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<CosmosDbOptions>>();
        CosmosDbOptions options = optionsMonitor.Get(serviceBindingName);
        return new CosmosClientBuilder(options.ConnectionString).WithApplicationName("cosmosdb-connector-sample").Build();
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

// Steeltoe: Insert some objects into CosmosDB collection.
await CosmosDbSeeder.CreateSampleDataAsync(app.Services);

app.Run();
