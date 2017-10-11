using Autofac;
using Autofac.Integration.Mvc;
using AutofacCloudFoundry.Models;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Steeltoe.Common.Options.Autofac;
using Steeltoe.Common.Configuration.Autofac;
using Pivotal.Extensions.Configuration.ConfigServer;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace AutofacCloudFoundry
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Get the configuration from the Spring Cloud Config Server using the "development" environment
            ApplicationConfig.RegisterConfig("development");

            var builder = new ContainerBuilder();

            // Register all the controllers with Autofac
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Add Options service to Autofac container
            builder.RegisterOptions();

            // Adds Options to Autofac container
            builder.RegisterCloudFoundryOptions(ApplicationConfig.Configuration);
            builder.RegisterConfigServerClientOptions(ApplicationConfig.Configuration);

            // Adds IConfiguration and IConfigurationRoot to Autofac container
            builder.RegisterConfiguration(ApplicationConfig.Configuration);

            // Add the configuration data returned from the Spring Cloud Config Server IOptions to Autofac container
            builder.RegisterOption<ConfigServerData>(ApplicationConfig.Configuration);

            // Create the Autofac container
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}
