using MySql.EntityFrameworkCore.Infrastructure;
using MySqlEFCore;
using MySqlEFCore.Data;
using Steeltoe.Configuration.CloudFoundry.ServiceBinding;
using Steeltoe.Connector.EntityFrameworkCore.MySql;
using Steeltoe.Connector.MySql;
using Steeltoe.Management.Endpoint;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Steeltoe: Add cloud service bindings.
builder.Configuration.AddCloudFoundryServiceBindings();

// Steeltoe: Add actuator endpoints.
builder.AddAllActuators();

// Steeltoe: Setup MySQL options, connection factory and health checks.
builder.AddMySql();

// Steeltoe: Review appsettings.development.json to see how local connection strings are provided.
bool useMultipleDatabases = builder.Configuration.GetValue("useMultipleDatabases", false);

if (useMultipleDatabases)
{
    // Steeltoe: When using multiple databases, specify the service binding name.
    const string serviceOneName = "myMySqlServiceOne";
    const string serviceTwoName = "myMySqlServiceTwo";

    // Steeltoe: optionally change the MySQL connection strings at runtime.
    builder.Services.Configure<MySqlOptions>(serviceOneName, options => options.ConnectionString += ";Use Compression=false");
    builder.Services.Configure<MySqlOptions>(serviceTwoName, options => options.ConnectionString += ";Use Compression=true");

    // Steeltoe: Setup DbContext connection strings, optionally changing MySQL options at runtime.
    builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseMySql(serviceProvider, serviceOneName,
        mySqlOptionsAction: untypedOptions =>
        {
            var mySqlOptions = (MySQLDbContextOptionsBuilder)untypedOptions;
            mySqlOptions.CommandTimeout(20);
        }));

    builder.Services.AddDbContext<OtherDbContext>((serviceProvider, options) => options.UseMySql(serviceProvider, serviceTwoName,
        mySqlOptionsAction: untypedOptions =>
        {
            var mySqlOptions = (MySQLDbContextOptionsBuilder)untypedOptions;
            mySqlOptions.CommandTimeout(25);
        }));
}
else
{
    // Steeltoe: optionally change the MySQL connection string at runtime.
    builder.Services.Configure<MySqlOptions>(options => options.ConnectionString += ";Use Compression=false");

    // Steeltoe: Setup DbContext connection string, optionally changing MySQL options at runtime.
    builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseMySql(serviceProvider, mySqlOptionsAction: untypedOptions =>
    {
        var mySqlOptions = (MySQLDbContextOptionsBuilder)untypedOptions;
        mySqlOptions.CommandTimeout(15);
    }));
}

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

// Steeltoe: Insert some rows into MySQL table.
await MySqlSeeder.CreateSampleDataAsync(app.Services);

if (useMultipleDatabases)
{
    await MySqlSeeder.CreateOtherSampleDataAsync(app.Services);
}

app.Run();
