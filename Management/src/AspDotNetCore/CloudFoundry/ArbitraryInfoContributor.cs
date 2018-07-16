using Steeltoe.Management.Endpoint.Info;

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
        /// <param name="builder"></param>
        public void Contribute(IInfoBuilder builder)
        {
            // pass in the info 
            builder.WithInfo("arbitraryInfo", new { someProperty = "someValue" });
        }
    }
}
