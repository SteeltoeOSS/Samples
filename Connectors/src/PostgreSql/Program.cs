using Npgsql;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Connectors.PostgreSql;
using Steeltoe.Management.Endpoint.Actuators.All;
using Steeltoe.Samples.PostgreSql;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Steeltoe: Add Cloud Foundry Configuration Provider for Actuator integration (not required for connectors)
builder.AddCloudFoundryConfiguration();

// Steeltoe: Add actuator endpoints.
builder.Services.AddAllActuators();

// Steeltoe: Setup PostgreSQL options, connection factory and health checks.
builder.AddPostgreSql();

// Steeltoe: optionally change the PostgreSQL connection string at runtime.
builder.Services.Configure<PostgreSqlOptions>(options =>
{
    var connectionStringBuilder = new NpgsqlConnectionStringBuilder
    {
        ConnectionString = options.ConnectionString,
        IncludeErrorDetail = true
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

// Steeltoe: Insert some rows into PostgreSQL table.
await PostgreSqlSeeder.CreateSampleDataAsync(app.Services);

app.Run();
