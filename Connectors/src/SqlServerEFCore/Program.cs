using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Connectors.EntityFrameworkCore.SqlServer;
using Steeltoe.Connectors.SqlServer;
using Steeltoe.Management.Endpoint.Actuators.All;
using Steeltoe.Samples.SqlServerEFCore;
using Steeltoe.Samples.SqlServerEFCore.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Steeltoe: Add Cloud Foundry Configuration Provider for Actuator integration (not required for connectors).
builder.AddCloudFoundryConfiguration();

// Steeltoe: Add actuator endpoints.
builder.Services.AddAllActuators();

// Steeltoe: Setup SQL Server options, connection factory and health checks.
builder.AddSqlServer();

// Steeltoe: optionally change the SQL Server connection string at runtime.
builder.Services.Configure<SqlServerOptions>(options =>
{
    var connectionStringBuilder = new SqlConnectionStringBuilder
    {
        ConnectionString = options.ConnectionString,
        ["Max Pool Size"] = 50
    };

    options.ConnectionString = connectionStringBuilder.ConnectionString;
});

// Steeltoe: Setup DbContext connection string, optionally changing SQL Server options at runtime.
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseSqlServer(serviceProvider, null, untypedOptions =>
{
    var sqlServerOptions = (SqlServerDbContextOptionsBuilder)untypedOptions;
    sqlServerOptions.EnableRetryOnFailure();
}));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();

// Steeltoe: Insert some rows into SQL Server table.
await SqlServerSeeder.CreateSampleDataAsync(app.Services);

app.Run();
