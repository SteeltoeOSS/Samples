using System.Runtime.InteropServices;
using Steeltoe.Common.HealthChecks;

namespace Steeltoe.Samples.FileSharesWeb;

internal class FileShareHealthContributor : IHealthContributor
{
    private const ulong ThresholdInBytes = 10 * 1024 * 1024;
    public string Id => "fileShareHealthContributor";

    public Task<HealthCheckResult?> CheckHealthAsync(CancellationToken cancellationToken)
    {
        HealthCheckResult result = Health();
        return Task.FromResult<HealthCheckResult?>(result);
    }

    private static HealthCheckResult Health()
    {
        if (!string.IsNullOrEmpty(FileShareHostedService.Location) && Directory.Exists(FileShareHostedService.Location))
        {
            if (NativeMethods.GetDiskFreeSpaceEx(FileShareHostedService.Location, out ulong freeBytesAvailable, out ulong totalNumberOfBytes,
                out ulong totalNumberOfFreeBytes))
            {
                var result = new HealthCheckResult
                {
                    Status = totalNumberOfFreeBytes >= ThresholdInBytes ? HealthStatus.Up : HealthStatus.Down
                };

                result.Details.Add("totalBytes", totalNumberOfBytes);
                result.Details.Add("freeBytes", totalNumberOfFreeBytes);
                result.Details.Add("freeBytesAvailable", freeBytesAvailable);
                result.Details.Add("minimumFreeBytes", ThresholdInBytes);
                result.Details.Add("path", FileShareHostedService.Location);
                return result;
            }

            int errorCode = Marshal.GetLastWin32Error();

            return new HealthCheckResult
            {
                Status = HealthStatus.Down,
                Description = "Failed to check free space.",
                Details =
                {
                    ["error"] = $"GetDiskFreeSpaceEx failed with error code {errorCode}.",
                    ["path"] = FileShareHostedService.Location
                }
            };
        }

        return new HealthCheckResult
        {
            Status = HealthStatus.Unknown,
            Description = "Unable to determine file share health.",
            Details =
            {
                ["error"] = "The configured path is invalid or does not exist.",
                ["path"] = FileShareHostedService.Location!
            }
        };
    }
}
