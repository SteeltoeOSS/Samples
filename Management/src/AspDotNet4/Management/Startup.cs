using Autofac;
using Management.App_Start;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Owin;
using Owin;
using Steeltoe.Common.Configuration.Autofac;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Logging;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.Endpoint.Health.Contributor;
using Steeltoe.Management.EndpointOwin;
using Steeltoe.Management.EndpointOwin.CloudFoundry;
using Steeltoe.Management.EndpointOwin.Env;
using Steeltoe.Management.EndpointOwin.Health;
using Steeltoe.Management.EndpointOwin.HeapDump;
using Steeltoe.Management.EndpointOwin.Info;
using Steeltoe.Management.EndpointOwin.Loggers;
using Steeltoe.Management.EndpointOwin.Metrics;
using Steeltoe.Management.EndpointOwin.Refresh;
using Steeltoe.Management.EndpointOwin.ThreadDump;
using Steeltoe.Management.EndpointOwin.Trace;
using System.Collections.Generic;
using System.Web.Hosting;

[assembly: OwinStartup(typeof(Management.Startup))]

namespace Management
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ApplicationConfig.Register("development");
            ApplicationConfig.ConfigureLogging();

            var startupLogger = ApplicationConfig.LoggerFactory.CreateLogger<Startup>();

            startupLogger.LogTrace("Configuring OWIN Pipeline");

            app
                .UseCloudFoundrySecurityMiddleware(ApplicationConfig.Configuration, null, ApplicationConfig.LoggerFactory)
                .UseCloudFoundryEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory)
                .UseEnvEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory)
                .UseHealthEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory)
                .UseHeapDumpEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory)
                .UseInfoEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory)
                .UseLoggersEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerProvider, ApplicationConfig.LoggerFactory)
                .UseMetricsEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory)
                .UseRefreshEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory)
                .UseThreadDumpEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory)
                .UseTraceEndpointMiddleware(ApplicationConfig.Configuration, null, ApplicationConfig.LoggerFactory);

            // using autofac...
            //var builder = new ContainerBuilder();
            //builder.RegisterCloudFoundryOptions(ApplicationConfig.Configuration);
            //builder.RegisterConfiguration(ApplicationConfig.Configuration);
            //builder.RegisterHealthComponents(ApplicationConfig.Configuration);

            //var container = builder.Build();
            //app.UseAutofacMiddleware(container);

            startupLogger.LogTrace("Application is online!");
        }
    }
}
