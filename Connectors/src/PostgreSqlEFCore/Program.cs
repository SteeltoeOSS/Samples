using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Steeltoe.Connectors.EntityFrameworkCore.PostgreSql;
using Steeltoe.Connectors.PostgreSql;
using Steeltoe.Management.Endpoint;
using Steeltoe.Samples.PostgreSqlEFCore;
using Steeltoe.Samples.PostgreSqlEFCore.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Steeltoe: Add actuator endpoints.
builder.AddAllActuators();

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

// Steeltoe: Setup DbContext connection string, optionally changing PostgreSQL options at runtime.
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseNpgsql(serviceProvider, null, untypedOptions =>
{
    var postgreSqlOptions = (NpgsqlDbContextOptionsBuilder)untypedOptions;
    postgreSqlOptions.CommandTimeout(15);
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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

// Steeltoe: Insert some rows into PostgreSQL table.
await PostgreSqlSeeder.CreateSampleDataAsync(app.Services);

app.Run();
