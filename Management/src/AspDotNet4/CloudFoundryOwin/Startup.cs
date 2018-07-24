using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
using Steeltoe.Management.Endpoint.Metrics;
using Steeltoe.Management.EndpointOwin;
using Steeltoe.Management.EndpointOwin.Metrics;
using Steeltoe.Management.Exporter.Metrics;
using Steeltoe.Management.Exporter.Metrics.CloudFoundryForwarder;
using System.Collections.Generic;
using System.Web.Http;

[assembly: OwinStartup(typeof(CloudFoundryOwin.Startup))]

namespace CloudFoundryOwin
{
    public class Startup
    {
        private IMetricsExporter MetricsExporter { get; set; }

        public void Configuration(IAppBuilder app)
        {
            var config = GlobalConfiguration.Configuration;

            app.UseCloudFoundryActuators(
                ApplicationConfig.Configuration,
                GetHealthContributors(),
                config.Services.GetApiExplorer(),
                ApplicationConfig.LoggerProvider,
                ApplicationConfig.LoggerFactory);

            // Uncomment if you want to enable metrics actuator endpoint, it's not enabled by default
            // app.UseMetricsActuator(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory);

            //  Uncomment if you want to enable exporting of metrics toCloud Foundry metrics exporter, it's not enabled by default
            // ConfigureMetricsExporter(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory);

            Start();
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

        private void ConfigureMetricsExporter(IConfiguration configuration, ILoggerFactory loggerFactory = null)
        {
            var options = new CloudFoundryForwarderOptions(configuration);
            MetricsExporter = new CloudFoundryForwarderExporter(options, OpenCensusStats.Instance, loggerFactory != null ? loggerFactory.CreateLogger<CloudFoundryForwarderExporter>() : null);
        }

        private void Start()
        {
            DiagnosticsManager.Instance.Start();
            if (MetricsExporter != null)
            {
                MetricsExporter.Start();
            }
        }

        public void Stop()
        {
            DiagnosticsManager.Instance.Stop();
            if (MetricsExporter != null)
            {
                MetricsExporter.Stop();
            }
        }
    }
}
