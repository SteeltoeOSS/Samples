using FortuneTellerService4.Models;
using Microsoft.Extensions.Logging;
using System.Web.Http;


namespace FortuneTellerService4
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        private ILogger<WebApiApplication> logger;
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var config = GlobalConfiguration.Configuration;

            // Build application configuration
            ApplicationConfig.RegisterConfig("development");
            LoggingConfig.Register(ApplicationConfig.Configuration);
            logger = LoggingConfig.LoggerFactory.CreateLogger<WebApiApplication>();

     
            DiscoveryConfig.Register(ApplicationConfig.Configuration, null);
            logger.LogInformation("Discovery service started!");

            SampleData.InitializeFortunes();
            logger.LogInformation("Fortunes populated!");

            logger.LogInformation("Finished Application_Start");
        }

        protected void Application_End()
        {
            logger.LogInformation("Shutting down!");

            // Unregister current app with Service Discovery server
            DiscoveryConfig.DiscoveryClient.ShutdownAsync().Wait();
        }
    }
}
