using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using PostgreSqlEFCore;
using PostgreSqlEFCore.Data;
using Steeltoe.Configuration.CloudFoundry.ServiceBinding;
using Steeltoe.Configuration.Kubernetes.ServiceBinding;
using Steeltoe.Connector.EntityFrameworkCore.PostgreSql;
using Steeltoe.Connector.PostgreSql;
using Steeltoe.Management.Endpoint;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Steeltoe: Add cloud service bindings.
builder.Configuration.AddCloudFoundryServiceBindings();
builder.Configuration.AddKubernetesServiceBindings();

// Steeltoe: Add actuator endpoints.
builder.AddAllActuators();

// Steeltoe: Setup PostgreSQL options, connection factory and health checks.
builder.AddPostgreSql();

// Steeltoe: optionally change the PostgreSQL connection string at runtime.
builder.Services.Configure<PostgreSqlOptions>(options => options.ConnectionString += ";Include Error Detail=true");

// Steeltoe: Setup DbContext connection string, optionally changing PostgreSQL options at runtime.
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseNpgsql(serviceProvider, npgsqlOptionsAction: untypedOptions =>
{
    var postgreSqlOptions = (NpgsqlDbContextOptionsBuilder)untypedOptions;
    postgreSqlOptions.CommandTimeout(15);
}));

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

// Steeltoe: Insert some rows into PostgreSQL table.
await PostgreSqlSeeder.CreateSampleDataAsync(app.Services);

app.Run();
