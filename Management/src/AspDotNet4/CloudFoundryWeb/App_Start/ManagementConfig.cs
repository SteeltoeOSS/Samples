using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Web.Http.Description;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Management.Endpoint;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.MySql;
using Steeltoe.CloudFoundry.Connector.Relational;
using Steeltoe.CloudFoundry.Connector.Relational.MySql;
using Steeltoe.CloudFoundry.Connector.Services;
using MySql.Data.MySqlClient;
using Steeltoe.Management.Endpoint.Health.Contributor;
using Steeltoe.Management.Exporter.Metrics;
using Steeltoe.Management.Exporter.Metrics.CloudFoundryForwarder;
using Steeltoe.Management.Endpoint.Metrics;

namespace CloudFoundryWeb
{
    public class ManagementConfig
    {
        public static IMetricsExporter MetricsExporter { get; set; }

        public static void ConfigureManagementActuators(IConfiguration configuration, ILoggerProvider dynamicLogger, IApiExplorer apiExplorer, ILoggerFactory loggerFactory = null)
        {
            ActuatorConfiguration.ConfigureForCloudFoundry(configuration, dynamicLogger, apiExplorer, loggerFactory);

            // You can individually configure actuators as shown below if you don't want all of them

            //ActuatorConfiguration.ConfigureSecurityService(configuration, null, loggerFactory);
            //ActuatorConfiguration.ConfigureCloudFoundryEndpoint(configuration, loggerFactory);
            //ActuatorConfiguration.ConfigureHealthEndpoint(configuration, null, GetHealthContributors(configuration), loggerFactory);
            //ActuatorConfiguration.ConfigureHeapDumpEndpoint(configuration, null, loggerFactory);
            //ActuatorConfiguration.ConfigureThreadDumpEndpoint(configuration, null, loggerFactory);
            //ActuatorConfiguration.ConfigureInfoEndpoint(configuration, null, loggerFactory);
            //ActuatorConfiguration.ConfigureLoggerEndpoint(configuration, dynamicLogger, loggerFactory);
            //ActuatorConfiguration.ConfigureTraceEndpoint(configuration, null, loggerFactory);
            //ActuatorConfiguration.ConfigureMappingsEndpoint(configuration, apiExplorer, loggerFactory);

            // Uncomment if you want to enable metrics actuator endpoint, it's not enabled by default
            // ActuatorConfiguration.ConfigureMetricsEndpoint(configuration, loggerFactory);

        }

        public static void ConfigureMetricsExporter(IConfiguration configuration, ILoggerFactory loggerFactory = null)
        {
            var options = new CloudFoundryForwarderOptions(configuration);
            MetricsExporter = new CloudFoundryForwarderExporter(options, OpenCensusStats.Instance, loggerFactory != null ? loggerFactory.CreateLogger<CloudFoundryForwarderExporter>() : null);
        }

        public static void Start()
        {
            DiagnosticsManager.Instance.Start();
            if (MetricsExporter != null)
            {
                MetricsExporter.Start();
            }
        }

        public static void Stop()
        {
            DiagnosticsManager.Instance.Stop();
            if (MetricsExporter != null)
            {
                MetricsExporter.Stop();
            }
        }

        private static IEnumerable<IHealthContributor> GetHealthContributors(IConfiguration configuration)
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