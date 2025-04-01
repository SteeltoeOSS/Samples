using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Common.Net;
using Steeltoe.Samples.FileSharesWeb.Models;

namespace Steeltoe.Samples.FileSharesWeb;

internal partial class FileShareHealthContributor(IOptionsMonitor<FileShareOptions> fileShareOptionsMonitor) : IHealthContributor
{
    private static ulong ThresholdInBytes => 10 * 1024 * 1024;
    public string Id => "fileShareHealthContributor";

    public Task<HealthCheckResult?> CheckHealthAsync(CancellationToken cancellationToken)
    {
        HealthCheckResult? result = Health();
        return Task.FromResult(result);
    }

    private HealthCheckResult? Health()
    {
        FileShareOptions fileShareOptions = fileShareOptionsMonitor.CurrentValue;

        if (!string.IsNullOrEmpty(fileShareOptions.Location))
        {
            try
            {
                using var networkPath = new WindowsNetworkFileShare(fileShareOptions.Location, fileShareOptions.Credential);
            }
            catch (Exception exception)
            {
                return new HealthCheckResult
                {
                    Status = HealthStatus.Down,
                    Description = $"Failed to connect to file share at '{fileShareOptions.Location}'.",
                    Details =
                    {
                        ["error"] = exception.Message
                    }
                };
            }

            if (Directory.Exists(fileShareOptions.Location))
            {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0018 // Inline variable declaration
                // If GetDiskFreeSpaceEx fails, these variables will have a value instead of undefined.
                ulong freeBytesAvailable = 0, totalNumberOfBytes = 0, totalNumberOfFreeBytes = 0;
#pragma warning restore IDE0018 // Inline variable declaration
#pragma warning restore IDE0059 // Unnecessary assignment of a value

                if (GetDiskFreeSpaceEx(fileShareOptions.Location, out freeBytesAvailable, out totalNumberOfBytes, out totalNumberOfFreeBytes))
                {
                    var result = new HealthCheckResult
                    {
                        Status = totalNumberOfFreeBytes >= ThresholdInBytes ? HealthStatus.Up : HealthStatus.Down
                    };

                    result.Details.Add("totalBytes", totalNumberOfBytes);
                    result.Details.Add("freeBytes", totalNumberOfFreeBytes);
                    result.Details.Add("freeBytesAvailable", freeBytesAvailable);
                    result.Details.Add("minimumFreeBytes", ThresholdInBytes);
                    result.Details.Add("path", fileShareOptions.Location);
                    return result;
                }
            }
        }

        return new HealthCheckResult
        {
            Status = HealthStatus.Unknown,
            Description = $"Failed to determine file share health. The configured path is '{fileShareOptions.Location}'.",
            Details =
            {
                ["error"] = "The configured path is invalid or does not exist."
            }
        };
    }

    [LibraryImport("kernel32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetDiskFreeSpaceEx(string lpDirectoryName, out ulong lpFreeBytesAvailable, out ulong lpTotalNumberOfBytes,
        out ulong lpTotalNumberOfFreeBytes);
}
