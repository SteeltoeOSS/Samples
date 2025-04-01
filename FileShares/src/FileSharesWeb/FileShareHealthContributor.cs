using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Common.Net;
using Steeltoe.Configuration.CloudFoundry;

namespace Steeltoe.Samples.FileSharesWeb;

public class FileShareHealthContributor : IHealthContributor
{
    public string Id => "fileShareHealthContributor";
    private static ulong Threshold => 10 * 1024 * 1024;

    private readonly string _sharePath;
    private NetworkCredential ShareCredentials { get; }

    public FileShareHealthContributor(IOptions<CloudFoundryServicesOptions> serviceOptions)
    {
        if (!serviceOptions.Value.Services.TryGetValue("credhub", out IList<CloudFoundryService>? value))
        {
            throw new InvalidOperationException();
        }
        var credHubEntry = value.FirstOrDefault(service => service.Name == "sampleNetworkShare");
        _sharePath = credHubEntry?.Credentials["location"].Value ?? throw new InvalidOperationException("Network share path is required for this application.");

        var userName = credHubEntry.Credentials["username"].Value ?? throw new InvalidOperationException("File share username is required.");
        var password = credHubEntry.Credentials["password"].Value ?? throw new InvalidOperationException("File share password is required.");
        ShareCredentials = new NetworkCredential(userName, password);
    }

    public Task<HealthCheckResult?> CheckHealthAsync(CancellationToken cancellationToken)
    {
        HealthCheckResult? result = Health();
        return Task.FromResult(result);
    }

    private HealthCheckResult? Health()
    {
        if (!string.IsNullOrEmpty(_sharePath))
        {
            try
            {
                using var networkPath = new WindowsNetworkFileShare(_sharePath, ShareCredentials);
            }
            catch(Exception e)
            {
                return new HealthCheckResult
                {
                    Status = HealthStatus.Down,
                    Description = "Failed to determine file share health.",
                    Details =
                    {
                        ["error"] = e.Message
                    }
                };
            }

            string absolutePath = Path.GetFullPath(_sharePath);

            if (Directory.Exists(absolutePath))
            {
                if (GetDiskFreeSpaceEx(_sharePath, out var freeBytesAvailable, out var totalNumberOfBytes, out var totalNumberOfFreeBytes))
                {
                    var result = new HealthCheckResult
                    {
                        Status = totalNumberOfFreeBytes >= Threshold ? HealthStatus.Up : HealthStatus.Down
                    };

                    result.Details.Add("totalBytes", totalNumberOfBytes);
                    result.Details.Add("freeBytes", totalNumberOfFreeBytes);
                    result.Details.Add("freeBytesAvailable", freeBytesAvailable);
                    result.Details.Add("threshold", Threshold);
                    result.Details.Add("path", absolutePath);
                    return result;
                }
            }
        }

        return new HealthCheckResult
        {
            Status = HealthStatus.Unknown,
            Description = "Failed to determine file share health.",
            Details =
            {
                ["error"] = "The configured path is invalid or does not exist."
            }
        };
    }

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool GetDiskFreeSpaceEx(string lpDirectoryName, out ulong lpFreeBytesAvailable, out ulong lpTotalNumberOfBytes, out ulong lpTotalNumberOfFreeBytes);
}
