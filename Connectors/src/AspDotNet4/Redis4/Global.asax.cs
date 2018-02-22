using Autofac;
using Autofac.Integration.Mvc;
using Redis4.Models;
using Steeltoe.CloudFoundry.ConnectorAutofac;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Redis4
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ServerConfig.RegisterConfig("development");
            var builder = new ContainerBuilder();

            // Register all the controllers with Autofac
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterDistributedRedisCache(ServerConfig.Configuration);
            builder.RegisterRedisConnectionMultiplexer(ServerConfig.Configuration);

            // Create the Autofac container
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            SampleData.InitializeCache(container);
        }
    }
}
