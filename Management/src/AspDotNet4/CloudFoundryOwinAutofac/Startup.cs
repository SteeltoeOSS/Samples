using Microsoft.Extensions.Logging;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CloudFoundryOwinAutofac.Startup))]

namespace CloudFoundryOwinAutofac
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var startupLogger = ApplicationConfig.LoggerFactory.CreateLogger<Startup>();

            startupLogger.LogTrace("Configuring OWIN Pipeline");

            // using autofac...

            // app cors config here is not needed, but does not interfere with Steeltoe config
            // app.UseCors(CorsOptions.AllowAll);
            app.UseAutofacMiddleware(ApplicationConfig.Container);

            startupLogger.LogTrace("Application is online!");
        }
    }
}
