using MySqlEFCore;
using MySqlEFCore.Data;
using Steeltoe.Connector.MySql;
using Steeltoe.Connector.MySql.EFCore;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Steeltoe: Setup
builder.AddCloudFoundryConfiguration();
builder.AddAllActuators();

// Steeltoe: MySQL EF Core Setup.
bool useMultipleDatabases = builder.Configuration.GetValue<bool>("useMultipleDatabases");

if (useMultipleDatabases)
{
    // When using multiple databases, specify the service binding name.
    // Review appsettings.development.json to see how local connection info is provided.

    const string serviceName = "myMySqlService";
    builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(builder.Configuration, serviceName));
    builder.Services.AddMySqlHealthContributor(builder.Configuration, serviceName);

    const string otherServiceName = "myOtherMySqlService";
    builder.Services.AddDbContext<OtherDbContext>(options => options.UseMySql(builder.Configuration, otherServiceName));
    builder.Services.AddMySqlHealthContributor(builder.Configuration, otherServiceName);
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(builder.Configuration));
    builder.Services.AddMySqlHealthContributor(builder.Configuration);
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
