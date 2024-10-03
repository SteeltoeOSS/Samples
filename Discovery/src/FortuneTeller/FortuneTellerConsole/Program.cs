using Steeltoe.Discovery.Configuration;
using Steeltoe.Discovery.Consul;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Discovery.HttpClients;
using Steeltoe.Samples.FortuneTellerConsole;
using Steeltoe.Samples.FortuneTellerConsole.Services;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

// Steeltoe: Add service discovery clients for Consul, Eureka, and configuration-based.
builder.Services.AddConsulDiscoveryClient();
builder.Services.AddEurekaDiscoveryClient();
builder.Services.AddConfigurationDiscoveryClient();

// Steeltoe: Register HTTP client to use service discovery.
builder.Services.AddHttpClient<FortuneService>(httpClient => httpClient.BaseAddress = new Uri("https://fortuneService/api/fortunes/")).AddServiceDiscovery();

IHost host = builder.Build();
host.Run();
