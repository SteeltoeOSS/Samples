using Autofac;
using Autofac.Integration.WebApi;
using FortuneTellerService4.Models;
using Pivotal.Discovery.Client;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Web.Http;
using System;
using Microsoft.Extensions.Logging.Console;

namespace FortuneTellerService4
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IDiscoveryClient _client;

        protected void Application_Start()
        {
            LoggerFactory factory = new LoggerFactory();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            var config = GlobalConfiguration.Configuration;

            // Build application configuration
            ServerConfig.RegisterConfig("development");

            // Add Console logging provider
            var settings = new ConfigurationConsoleLoggerSettings(ServerConfig.Configuration.GetSection("Logging"));
            factory.AddProvider(new ConsoleLoggerProvider(settings));

            ILogger<WebApiApplication> logger = factory.CreateLogger<WebApiApplication>();
            logger.LogInformation("Starting to build container");

            // Create IOC container builder
            var builder = new ContainerBuilder();

            // Register logger factory
            builder.RegisterLoggingFactory(factory);

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register IDiscoveryClient, etc.
            builder.RegisterDiscoveryClient(ServerConfig.Configuration, factory);

            // Initialize and Register FortuneContext
            builder.RegisterInstance(SampleData.InitializeFortunes()).SingleInstance();

            // Register FortuneRepository
            builder.RegisterType<FortuneRepository>().As<IFortuneRepository>().SingleInstance();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            logger.LogInformation("Finished container build, starting background services");

            // Start the Discovery client background thread
            _client = container.Resolve<IDiscoveryClient>();

            logger.LogInformation("Finished starting background services");
        }

        protected void Application_End()
        {
            // Unregister current app with Service Discovery server
            _client.ShutdownAsync();
        }
    }
}
