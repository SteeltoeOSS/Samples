using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Connectors.RabbitMQ;
using Steeltoe.Management.Endpoint.Actuators.All;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Steeltoe: Add Cloud Foundry Configuration Provider for Actuator integration (not required for connectors).
builder.AddCloudFoundryConfiguration();

// Steeltoe: Add actuator endpoints.
builder.Services.AddAllActuators();

// Steeltoe: Setup RabbitMQ options, connection factory and health checks.
builder.AddRabbitMQ(null, addOptions =>
{
    // Optionally provide a callback to customize client settings.
    addOptions.CreateConnection = (serviceProvider, serviceBindingName) =>
    {
        var optionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<RabbitMQOptions>>();
        RabbitMQOptions options = optionsMonitor.Get(serviceBindingName);

        var factory = new ConnectionFactory
        {
            ClientProvidedName = "rabbitmq-connector-sample"
        };

        if (options.ConnectionString != null)
        {
            factory.Uri = new Uri(options.ConnectionString);
        }

        Task<IConnection> connectionTask = factory.CreateConnectionAsync();
        return connectionTask.GetAwaiter().GetResult();
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

app.Run();
