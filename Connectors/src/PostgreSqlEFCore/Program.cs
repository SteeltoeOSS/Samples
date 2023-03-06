using PostgreSqlEFCore;
using PostgreSqlEFCore.Data;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Configuration.Kubernetes.ServiceBinding;
using Steeltoe.Connector.EntityFrameworkCore.PostgreSql;
using Steeltoe.Connector.PostgreSql;
using Steeltoe.Management.Endpoint;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Steeltoe: Add cloud service bindings.
builder.AddCloudFoundryConfiguration();
builder.Configuration.AddKubernetesServiceBindings();

// Steeltoe: Add actuator endpoints.
builder.AddAllActuators();

// Steeltoe: Setup PostgreSQL options, connection factory, DbContext connection string and health checks.
builder.Services.AddPostgreSqlHealthContributor(builder.Configuration);
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration));

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
