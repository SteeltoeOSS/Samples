using Autofac;
using Autofac.Integration.Mvc;
using FortuneTellerUI4.Services;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using Steeltoe.Common.Logging.Autofac;
using Steeltoe.Common.Options.Autofac;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FortuneTellerUI4
{
    public class MvcApplication : System.Web.HttpApplication
    {
   

        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ApplicationConfig.RegisterConfig("development");

            var builder = new ContainerBuilder();

            // Add Microsoft Options to container
            builder.RegisterOptions();

            // Add Microsoft Logging to container
            builder.RegisterLogging(ApplicationConfig.Configuration);

            // Add Console logger to container
            builder.RegisterConsoleLogging();

            // Register all the controllers with Autofac
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register IDiscoveryClient, etc.
            builder.RegisterDiscoveryClient(ApplicationConfig.Configuration);

            // Register FortuneService
            builder.RegisterType<FortuneService>().As<IFortuneService>().SingleInstance();

            // Create the Autofac container
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Get a logger from container
            var logger = container.Resolve<ILogger<MvcApplication>>();

            logger.LogInformation("Finished container build, starting background services");

            // Start the Discovery client background thread
            container.StartDiscoveryClient();

            logger.LogInformation("Finished starting background services");
        }
    }
}
