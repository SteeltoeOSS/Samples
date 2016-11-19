using Autofac;
using Autofac.Integration.Mvc;
using AutofacCloudFoundry.Models;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

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
            ConfigServerConfig.RegisterConfig("development");

            var builder = new ContainerBuilder();

            // Register all the controllers with Autofac
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Adds IConfigurationRoot as a service and also adds 
            // IOption<ConfigServerClientSettingsOptions>, IOption<CloudFoundryApplicationOptions>, IOption<CloudFoundryServicesOptions>
            builder.RegisterConfigServer(ConfigServerConfig.Configuration);

            // Add the configuration data returned from the Spring Cloud Config Server as a service
            builder.Configure<ConfigServerData>(ConfigServerConfig.Configuration);

            // Create the Autofac container
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}
