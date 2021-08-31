using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Owin;
using Owin;
using Steeltoe.CloudFoundry.Connector.Relational;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Management.Census.Stats;
using Steeltoe.Management.Endpoint.Health.Contributor;
using Steeltoe.Management.EndpointOwin;
using Steeltoe.Management.EndpointOwin.Metrics;
using Steeltoe.Management.EndpointOwin.Refresh;
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
                GetHealthContributors(ApplicationConfig.Configuration),
                config.Services.GetApiExplorer(),
                LoggingConfig.LoggerProvider,
                LoggingConfig.LoggerFactory);

            // Some actuator endpoints are not enabled by UseCloudFoundryActuators
            app.UseMetricsActuator(ApplicationConfig.Configuration, LoggingConfig.LoggerFactory);
            app.UseRefreshActuator(ApplicationConfig.Configuration, LoggingConfig.LoggerFactory);

            //  Uncomment if you want to enable exporting of metrics to Cloud Foundry metrics exporter, it's not enabled by default
            // UseCloudFoundryMetricsExporter(ApplicationConfig.Configuration, LoggingConfig.LoggerFactory);

            Start();
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

        private void UseCloudFoundryMetricsExporter(IConfiguration configuration, ILoggerFactory loggerFactory = null)
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
