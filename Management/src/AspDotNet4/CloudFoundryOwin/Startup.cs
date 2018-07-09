using CloudFoundryOwin.App_Start;
using Microsoft.Extensions.Logging;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CloudFoundryOwin.Startup))]

namespace CloudFoundryOwin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
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

            // app cors config here is not needed, but does not interfere with Steeltoe config
            // app.UseCors(CorsOptions.AllowAll);
            app.UseAutofacMiddleware(ApplicationConfig.Container);

            startupLogger.LogTrace("Application is online!");
        }
    }
}
