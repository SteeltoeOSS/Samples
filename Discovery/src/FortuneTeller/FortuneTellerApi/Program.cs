using Microsoft.EntityFrameworkCore;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Configuration.CloudFoundry.ServiceBindings;
using Steeltoe.Configuration.ConfigServer;
using Steeltoe.Discovery.Consul;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Management.Endpoint;
using Steeltoe.Samples.FortuneTellerApi;
using Steeltoe.Samples.FortuneTellerApi.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Steeltoe: Store sample fortunes in memory.
builder.Services.AddDbContext<FortuneDbContext>(options => options.UseInMemoryDatabase("Fortunes"));
builder.Services.AddHostedService<DatabaseSeederHostedService>();

// Steeltoe: Read credentials to Eureka server from VCAP_SERVICES on Cloud Foundry.
builder.AddCloudFoundryConfiguration();
builder.Configuration.AddCloudFoundryServiceBindings();

// Steeltoe: Add service discovery clients for Consul and Eureka.
builder.Services.AddConsulDiscoveryClient();
builder.Services.AddEurekaDiscoveryClient();

// Steeltoe: Add Config Server to demonstrate discovery-first (resolves the URL to Config Server from Eureka).
builder.Configuration.AddConfigServer();

// Steeltoe: Add actuator endpoints.
builder.AddAllActuators();

// Steeltoe: Change line below to HealthStatus.Down to observe this app marked as DOWN in discovery server (using *Actuator launch profile).
builder.Services.AddSingleton<IHealthContributor>(_ => new ExampleHealthContributor(HealthStatus.Up));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
