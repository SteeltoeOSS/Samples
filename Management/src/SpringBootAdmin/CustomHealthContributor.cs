using Steeltoe.Common.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace CloudFoundry
{
    /// <summary>
    /// This is an example of a custom HealthContributor
    /// </summary>
    public class CustomHealthContributor : IHealthContributor
    {
        public string Id => "CustomHealthContributor";

        public Task<HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken)
        {
            var result = new HealthCheckResult {
                Status = HealthStatus.Up, // this is used as part of the aggregate, it is not directly part of the middleware response
                Description = "This health check does not check anything"
            };
            result.Details.Add("status", HealthStatus.Up.ToString());

            return Task.FromResult(result);
        }
    }
}
