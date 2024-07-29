using Steeltoe.Management.Endpoint.Info;
using Steeltoe.Management.Info;
using System.Threading;
using System.Threading.Tasks;

namespace CloudFoundry
{
    /// <summary>
    /// This is an example of an extremely basic IInfoContributor
    /// </summary>
    public class ArbitraryInfoContributor : IInfoContributor
    {
        /// <summary>
        /// This is where you add your information
        /// </summary>
        public Task ContributeAsync(IInfoBuilder builder, CancellationToken cancellationToken)
        {
            builder.WithInfo("arbitraryInfo", new { someProperty = "someValue" });
            return Task.CompletedTask;
        }
    }
}
