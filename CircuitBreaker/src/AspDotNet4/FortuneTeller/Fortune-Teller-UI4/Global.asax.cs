using Autofac;
using Autofac.Integration.Mvc;
using FortuneTellerUI4.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Pivotal.Discovery.Client;
using Steeltoe.CircuitBreaker.Hystrix.MetricsStream;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FortuneTellerUI4
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IDiscoveryClient _client;
        private HystrixMetricsStreamPublisher _publisher;

        protected void Application_Start()
        {

            ILoggerFactory factory = new LoggerFactory();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ServerConfig.RegisterConfig("development");

            // Add Console logging provider
            var settings = new ConfigurationConsoleLoggerSettings(ServerConfig.Configuration.GetSection("Logging"));
            factory.AddProvider(new ConsoleLoggerProvider(settings));

            ILogger<MvcApplication> logger = factory.CreateLogger<MvcApplication>();
            logger.LogInformation("Starting to build container");

            var builder = new ContainerBuilder();

            // Register all the controllers with Autofac
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register logger factory
            builder.RegisterLoggingFactory(factory);

            // Register IDiscoveryClient, etc.
            builder.RegisterDiscoveryClient(ServerConfig.Configuration, factory); 

            // Register FortuneService Hystrix command
            builder.RegisterHystrixCommand<IFortuneService,FortuneService>("fortuneService", ServerConfig.Configuration);

            // Register Hystrix Metrics/Monitoring stream
            builder.RegisterHystrixMetricsStream(ServerConfig.Configuration);

            // Create the Autofac container
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            logger.LogInformation("Finished container build, starting background services");

            // Start the Discovery client background thread
            _client = container.Resolve<IDiscoveryClient>();

            // Start the Hystrix Metrics stream 
            _publisher = container.Resolve<HystrixMetricsStreamPublisher>();

            logger.LogInformation("Finished starting background services");

        }
    }
}
