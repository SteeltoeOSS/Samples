using Steeltoe.Common.HealthChecks;

namespace CloudFoundry
{
    /// <summary>
    /// This is an example of a custom HealthContributor
    /// </summary>
    public class CustomHealthContributor : IHealthContributor
    {
        public string Id => "CustomHealthContributor";


        public HealthCheckResult Health()
        {
            var result = new HealthCheckResult {
                Status = HealthStatus.UP, // this is used as part of the aggregate, it is not directly part of the middleware response
                Description = "This health check does not check anything"
            };
            result.Details.Add("status", HealthStatus.UP.ToString());
            return result;
        }
    }
}
