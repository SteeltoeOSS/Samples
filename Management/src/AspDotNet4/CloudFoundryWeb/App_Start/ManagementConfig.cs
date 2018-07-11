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

namespace CloudFoundryWeb
{
    public class ManagementConfig
    {
        public static void ConfigureManagementActuators(IConfiguration configuration, ILoggerProvider dynamicLogger, IApiExplorer apiExplorer, ILoggerFactory loggerFactory = null)
        {
            // ActuatorConfiguration.ConfigureForCloudFoundry(configuration, dynamicLogger, apiExplorer, loggerFactory);

            ActuatorConfiguration.ConfigureSecurityService(configuration, null, loggerFactory);
            ActuatorConfiguration.ConfigureCloudFoundryEndpoint(configuration, loggerFactory);
            ActuatorConfiguration.ConfigureHealthEndpoint(configuration, null, GetHealthContributors(configuration), loggerFactory);
            ActuatorConfiguration.ConfigureHeapDumpEndpoint(configuration, null, loggerFactory);
            ActuatorConfiguration.ConfigureThreadDumpEndpoint(configuration, null, loggerFactory);
            ActuatorConfiguration.ConfigureInfoEndpoint(configuration, null, loggerFactory);
            ActuatorConfiguration.ConfigureLoggerEndpoint(configuration, dynamicLogger, loggerFactory);
            ActuatorConfiguration.ConfigureTraceEndpoint(configuration, null, loggerFactory);
            ActuatorConfiguration.ConfigureMappingsEndpoint(configuration, apiExplorer, loggerFactory);
        }
        public static void Start()
        {
            DiagnosticsManager.Instance.Start();
        }

        public static void Stop()
        {
            DiagnosticsManager.Instance.Stop();
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