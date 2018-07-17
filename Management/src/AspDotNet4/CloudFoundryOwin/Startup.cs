using Microsoft.Owin;
using MySql.Data.MySqlClient;
using Owin;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.MySql;
using Steeltoe.CloudFoundry.Connector.Relational;
using Steeltoe.CloudFoundry.Connector.Relational.MySql;
using Steeltoe.CloudFoundry.Connector.Services;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Management.Endpoint.Health.Contributor;
using Steeltoe.Management.EndpointOwin;
using System.Collections.Generic;
using System.Web.Http;

[assembly: OwinStartup(typeof(CloudFoundryOwin.Startup))]

namespace CloudFoundryOwin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = GlobalConfiguration.Configuration;

            app.UseCloudFoundryActuators(
                ApplicationConfig.Configuration,
                GetHealthContributors(),
                config.Services.GetApiExplorer(),
                ApplicationConfig.LoggerProvider,
                ApplicationConfig.LoggerFactory);

            DiagnosticsManager.Instance.Start();
        }

        private IEnumerable<IHealthContributor> GetHealthContributors()
        {
            var info = ApplicationConfig.Configuration.GetSingletonServiceInfo<MySqlServiceInfo>();
            var mySqlConfig = new MySqlProviderConnectorOptions(ApplicationConfig.Configuration);
            var factory = new MySqlProviderConnectorFactory(info, mySqlConfig, MySqlTypeLocator.MySqlConnection);

            var healthContributors = new List<IHealthContributor>
            {
                new DiskSpaceContributor(),
                new RelationalHealthContributor(new MySqlConnection(factory.CreateConnectionString()))
            };

            return healthContributors;
        }
    }
}
