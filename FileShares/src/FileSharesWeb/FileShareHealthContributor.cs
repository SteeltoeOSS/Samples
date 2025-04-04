using System.ComponentModel;
using System.Runtime.InteropServices;
using Steeltoe.Common.HealthChecks;

namespace Steeltoe.Samples.FileSharesWeb;

internal class FileShareHealthContributor(FileShareConfiguration fileShareConfiguration) : IHealthContributor
{
    private const ulong ThresholdInBytes = 10 * 1024 * 1024;
    private readonly FileShareConfiguration _fileShareConfiguration = fileShareConfiguration;
    public string Id => "fileShareHealthContributor";

    public Task<HealthCheckResult?> CheckHealthAsync(CancellationToken cancellationToken)
    {
        HealthCheckResult result = Health();
        return Task.FromResult<HealthCheckResult?>(result);
    }

    private HealthCheckResult Health()
    {
        if (!Directory.Exists(_fileShareConfiguration.Location))
        {
            return new HealthCheckResult
            {
                Status = HealthStatus.Unknown,
                Description = "Unable to determine file share health.",
                Details =
                {
                    ["error"] = "The configured path is invalid or does not exist.",
                    ["path"] = _fileShareConfiguration.Location!
                }
            };
        }

        if (NativeMethods.GetDiskFreeSpaceEx(_fileShareConfiguration.Location, out ulong freeBytesAvailable, out ulong totalNumberOfBytes,
            out ulong totalNumberOfFreeBytes))
        {
            var result = new HealthCheckResult
            {
                Status = totalNumberOfFreeBytes >= ThresholdInBytes ? HealthStatus.Up : HealthStatus.Down
            };

            result.Details.Add("bytesFreeForUser", freeBytesAvailable);
            result.Details.Add("totalFreeBytes", totalNumberOfFreeBytes);
            result.Details.Add("minimumFreeBytes", ThresholdInBytes);
            result.Details.Add("totalCapacityBytes", totalNumberOfBytes);
            result.Details.Add("path", _fileShareConfiguration.Location);
            return result;
        }

        int errorCode = Marshal.GetLastWin32Error();
        var exception = new Win32Exception(errorCode);

        return new HealthCheckResult
        {
            Status = HealthStatus.Down,
            Description = "Failed to check free space.",
            Details =
            {
                ["error"] = exception.ToString(),
                ["path"] = _fileShareConfiguration.Location
            }
        };
    }
}
