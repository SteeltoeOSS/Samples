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
            ActuatorConfigurator.UseCloudFoundryActuators(configuration, dynamicLogger, GetHealthContributors(configuration), apiExplorer, loggerFactory);

            // You can individually configure actuators as shown below if you don't want all of them
            //ActuatorConfigurator.UseCloudFoundrySecurity(configuration, null, loggerFactory);
            //ActuatorConfigurator.UseCloudFoundryActuator(configuration, loggerFactory);
            //ActuatorConfigurator.UseHealthActuator(configuration, null, GetHealthContributors(configuration), loggerFactory);
            //ActuatorConfigurator.UseHeapDumpActuator(configuration, null, loggerFactory);
            //ActuatorConfigurator.UseThreadDumpActuator(configuration, null, loggerFactory);
            //ActuatorConfigurator.UseInfoActuator(configuration, null, loggerFactory);
            //ActuatorConfigurator.UseLoggerActuator(configuration, dynamicLogger, loggerFactory);
            //ActuatorConfigurator.UseTraceActuator(configuration, null, loggerFactory);
            //ActuatorConfigurator.UseMappingsActuator(configuration, apiExplorer, loggerFactory);

            // Uncomment if you want to enable metrics actuator endpoint, it's not enabled by default
            // ActuatorConfigurator.UseMetricsActuator(configuration, loggerFactory);

        }

        public static void UseCloudFoundryMetricsExporter(IConfiguration configuration, ILoggerFactory loggerFactory = null)
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
            var healthContributors = new List<IHealthContributor>
            {
                new DiskSpaceContributor(),
                RelationalHealthContributor.GetMySqlContributor(configuration)
            };

            return healthContributors;
        }
    }
}