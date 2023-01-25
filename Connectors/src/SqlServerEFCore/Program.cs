using SqlServerEFCore;
using SqlServerEFCore.Data;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Connector.EntityFrameworkCore.SqlServer;
using Steeltoe.Connector.SqlServer;
using Steeltoe.Management.Endpoint;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Steeltoe: Setup
builder.AddCloudFoundryConfiguration();
builder.AddAllActuators();
builder.Services.AddSqlServerHealthContributor(builder.Configuration);
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration));

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

// Steeltoe: Insert some rows into SQL Server table.
await SqlServerSeeder.CreateSampleDataAsync(app.Services);

app.Run();
