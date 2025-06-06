using Microsoft.Extensions.Options;
using Steeltoe.Common;
using Steeltoe.Common.Certificates;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Samples.AuthConsole;
using Steeltoe.Samples.AuthConsole.ApiClients;
using Steeltoe.Security.Authorization.Certificate;

const string orgId = "a8fef16f-94c0-49e3-aa0b-ced7c3da6229";
const string spaceId = "122b942a-d7b9-4839-b26e-836654b9785f";

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddOptions<CloudFoundryConventions>().Bind(builder.Configuration.GetSection(CloudFoundryConventions.ConfigurationPrefix));
builder.Services.AddHostedService<Worker>();

// Steeltoe: Add Cloud Foundry application info and instance identity certificate to configuration.
builder.AddCloudFoundryConfiguration();
builder.Configuration.AddAppInstanceIdentityCertificate(new Guid(orgId), new Guid(spaceId));

builder.Services.AddHttpClient<CertificateAuthorizationApiClient>(SetBaseAddress).AddAppInstanceIdentityCertificate().ConfigureLogging();

IHost host = builder.Build();
host.Run();

return;

// This code is used to limit complexity in the sample. A real application should use Service Discovery.
// To learn more about service discovery, review the documentation at: https://docs.steeltoe.io/api/v4/discovery/
static void SetBaseAddress(IServiceProvider serviceProvider, HttpClient client)
{
    var instanceInfo = serviceProvider.GetRequiredService<IApplicationInstanceInfo>();

    if (instanceInfo is CloudFoundryApplicationOptions { Api: not null } options)
    {
        var conventions = serviceProvider.GetRequiredService<IOptions<CloudFoundryConventions>>();

        string? address = options.Api;
        ArgumentException.ThrowIfNullOrEmpty(options.Api);

        string baseAddress = address!.Replace(conventions.Value.ApiUriSegment, $"auth-server-sample.{conventions.Value.AppsUriSegment}");
        client.BaseAddress = new Uri($"{baseAddress}");
    }
    else
    {
        client.BaseAddress = new Uri("https://localhost:7184");
    }
}
