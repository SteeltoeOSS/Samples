using System.Net;
using Microsoft.Extensions.Options;
using Steeltoe.Common.Net;
using Steeltoe.Configuration.CloudFoundry;

namespace Steeltoe.Samples.FileSharesWeb;

public sealed class FileShareHostedService : IHostedService
{
    private readonly NetworkCredential _credential;
    private WindowsNetworkFileShare? _fileShare;
    internal static string? Location { get; private set; }

    public FileShareHostedService(IOptions<CloudFoundryServicesOptions> serviceOptions)
    {
        if (!serviceOptions.Value.Services.TryGetValue("credhub", out IList<CloudFoundryService>? value))
        {
            throw new InvalidOperationException("A 'credhub' service instance was not found in app configuration.");
        }

        CloudFoundryService? credHubEntry = value.FirstOrDefault(service => service.Name == "sampleNetworkShare");
        Location = credHubEntry?.Credentials["location"].Value ?? throw new InvalidOperationException("Network share path is required.");
        string userName = credHubEntry.Credentials["username"].Value ?? throw new InvalidOperationException("Network share username is required.");
        string password = credHubEntry.Credentials["password"].Value ?? throw new InvalidOperationException("Network share password is required.");
        _credential = new NetworkCredential(userName, password);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _fileShare = new WindowsNetworkFileShare(Location!, _credential);

        if (!Directory.Exists(Location))
        {
            throw new IOException($"File share path '{Location}' does not exist. Review the contents of README.md.");
        }

        // Perform a quick write and delete test to confirm the path and credentials are valid.
        // Unhandled exceptions will occur if credentials or permissions are incorrect.
        const string testFile = "manifest-windows.yml";

        if (File.Exists(testFile))
        {
            File.Copy(testFile, Path.Combine(Location, testFile));
            File.Delete(Path.Combine(Location, testFile));
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _fileShare?.Dispose();
        return Task.CompletedTask;
    }
}
