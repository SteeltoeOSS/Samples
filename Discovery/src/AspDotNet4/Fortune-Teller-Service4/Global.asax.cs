using Autofac;
using Autofac.Integration.WebApi;
using FortuneTellerService4.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Pivotal.Discovery.Client;
using Steeltoe.Common.Discovery;
using Steeltoe.Common.Logging.Autofac;
using Steeltoe.Common.Options.Autofac;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Web.Http;


namespace FortuneTellerService4
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IContainer container;

        protected void Application_Start()
        {

            GlobalConfiguration.Configure(WebApiConfig.Register);

            var config = GlobalConfiguration.Configuration;

            // Build application configuration
            ApplicationConfig.RegisterConfig("development");

            var builder = new ContainerBuilder();

            // Add Microsoft Options to container
            builder.RegisterOptions();

            // Add Microsoft Logging to container
            builder.RegisterLogging(ApplicationConfig.Configuration);

            // Add Console logger to container
            builder.RegisterConsoleLogging();

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register IDiscoveryClient, etc.
            builder.RegisterDiscoveryClient(ApplicationConfig.Configuration);

            // Initialize and Register FortuneContext
            builder.RegisterInstance(SampleData.InitializeFortunes()).SingleInstance();

            // Register FortuneRepository
            builder.RegisterType<FortuneRepository>().As<IFortuneRepository>().SingleInstance();

            container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Get a logger from container
            var logger = container.Resolve <ILogger<WebApiApplication>>();

            logger.LogInformation("Finished container build, starting background services");

            // Start the Discovery client background thread
            container.StartDiscoveryClient();

            logger.LogInformation("Finished starting background services");

        }

        protected void Application_End()
        {
            var client = container.Resolve<IDiscoveryClient>();
            var logger = container.Resolve<ILogger<WebApiApplication>>();

            logger.LogInformation("Shutting down!");

            // Unregister current app with Service Discovery server
            client.ShutdownAsync().Wait();
        }
    }
}
