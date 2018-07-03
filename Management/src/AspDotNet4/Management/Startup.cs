using Autofac;
using Management.App_Start;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using Steeltoe.CloudFoundry.ConnectorAutofac;
using Steeltoe.Common.Configuration.Autofac;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Logging;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.Endpoint.Health.Contributor;
using Steeltoe.Management.EndpointAutofac;
using Steeltoe.Management.EndpointAutofac.Actuators;
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

            // without dependency injection...
            //app
            //    .UseCloudFoundrySecurityMiddleware(ApplicationConfig.Configuration, null, ApplicationConfig.LoggerFactory)
            //    .UseCloudFoundryEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory)
            //    .UseThreadDumpEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory)
            //    .UseHeapDumpEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.GetContentRoot(), ApplicationConfig.LoggerFactory)
            //    .UseEnvEndpointOwinMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory)
            //    .UseInfoEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory)
            //    .UseHealthEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory)
            //    .UseLoggersEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerProvider, ApplicationConfig.LoggerFactory)
            //    .UseTraceEndpointMiddleware(ApplicationConfig.Configuration, null, ApplicationConfig.LoggerFactory) // totally not implemented!
            //    .UseMetricsEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory) // no metrics!
            //    .UseRefreshEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory);
            // .UseMappingEndpointMiddleware(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory); // not even started!

            // using autofac...
            var builder = new ContainerBuilder();
            builder.RegisterCloudFoundryOptions(ApplicationConfig.Configuration);
            builder.RegisterConfiguration(ApplicationConfig.Configuration);
            builder.RegisterMySqlConnection(ApplicationConfig.Configuration);
            builder.UseCloudFoundryMiddlewares(ApplicationConfig.Configuration);

            var container = builder.Build();

            // app cors config here is not needed, but does not interfere with Steeltoe config
            // app.UseCors(CorsOptions.AllowAll);
            app.UseAutofacMiddleware(container);


            startupLogger.LogTrace("Application is online!");
        }
    }
}
