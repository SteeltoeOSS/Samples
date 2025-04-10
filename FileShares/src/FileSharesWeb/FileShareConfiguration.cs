using System.Net;
using Microsoft.Extensions.Options;
using Steeltoe.Configuration.CloudFoundry;

namespace Steeltoe.Samples.FileSharesWeb;

public sealed class FileShareConfiguration
{
    internal string Location { get; }
    internal NetworkCredential Credential { get; }

    public FileShareConfiguration(IOptions<CloudFoundryServicesOptions> serviceOptions)
    {
        if (!serviceOptions.Value.Services.TryGetValue("credhub", out IList<CloudFoundryService>? value))
        {
            throw new InvalidOperationException("A 'credhub' service instance was not found in app configuration.");
        }

        CloudFoundryService? credHubEntry = value.FirstOrDefault(service => service.Name == "sampleNetworkShare");
        Location = credHubEntry?.Credentials["location"].Value ?? throw new InvalidOperationException("Network share path is required.");
        string userName = credHubEntry.Credentials["username"].Value ?? throw new InvalidOperationException("Network share username is required.");
        string password = credHubEntry.Credentials["password"].Value ?? throw new InvalidOperationException("Network share password is required.");
        Credential = new NetworkCredential(userName, password);
    }
}
