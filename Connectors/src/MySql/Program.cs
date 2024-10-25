using MySqlConnector;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Connectors.MySql;
using Steeltoe.Management.Endpoint.Actuators.All;
using Steeltoe.Samples.MySql;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Steeltoe: Add Cloud Foundry Configuration Provider for Actuator integration (not required for connectors)
builder.AddCloudFoundryConfiguration();

// Steeltoe: Add actuator endpoints.
builder.Services.AddAllActuators();

// Steeltoe: Setup MySQL options, connection factory and health checks.
builder.AddMySql();

// Steeltoe: optionally change the MySQL connection string at runtime.
builder.Services.Configure<MySqlOptions>(options =>
{
    var connectionStringBuilder = new MySqlConnectionStringBuilder
    {
        ConnectionString = options.ConnectionString,
        UseCompression = false
    };

    options.ConnectionString = connectionStringBuilder.ConnectionString;
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

// Steeltoe: Insert some rows into MySQL table.
await MySqlSeeder.CreateSampleDataAsync(app.Services);

app.Run();
