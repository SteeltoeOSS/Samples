using RabbitMQ.Client;
using Steeltoe.Configuration.CloudFoundry.ServiceBinding;
using Steeltoe.Connector.RabbitMQ;
using Steeltoe.Management.Endpoint;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Steeltoe: Add cloud service bindings.
builder.Configuration.AddCloudFoundryServiceBindings();

// Steeltoe: Add actuator endpoints.
builder.AddAllActuators();

// Steeltoe: Setup RabbitMQ options, connection factory and health checks, optionally providing a callback to customize client settings.
builder.AddRabbitMQ((options, _) =>
{
    var factory = new ConnectionFactory
    {
        ClientProvidedName = "rabbitmq-connector"
    };

    if (options.ConnectionString != null)
    {
        factory.Uri = new Uri(options.ConnectionString);
    }

    return factory.CreateConnection();
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

app.Run();
