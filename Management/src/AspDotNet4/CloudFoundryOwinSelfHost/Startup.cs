using MySql.Data.MySqlClient;
using Owin;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.MySql;
using Steeltoe.CloudFoundry.Connector.Relational;
using Steeltoe.CloudFoundry.Connector.Services;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Management.Endpoint.Health.Contributor;
using Steeltoe.Management.EndpointOwin;
using Steeltoe.Management.EndpointOwin.Metrics;
using Steeltoe.Management.EndpointOwin.Refresh;
using System.Collections.Generic;

namespace CloudFoundryOwinSelfHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            WebApiConfig.RegisterRoutes(app);

            app.UseCloudFoundryActuators(
                ApplicationConfig.Configuration,
                GetHealthContributors(),
                WebApiConfig.GetApiExplorer(),
                ApplicationConfig.LoggerProvider,
                ApplicationConfig.LoggerFactory);

            DiagnosticsManager.Instance.Start();

            app
                .UseMetricsActuator(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory)
                .UseRefreshActuator(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory);
            //    .UseEnvEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory)

            //DiagnosticsManager.Instance.Start();
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
