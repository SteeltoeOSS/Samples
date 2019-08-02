using Autofac;
using Autofac.Integration.Mvc;
using Steeltoe.CloudFoundry.ConnectorAutofac;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Gemfire
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ServerConfig.RegisterConfig("development");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();

            // Register all the controllers with Autofac
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterGemFireConnection(ServerConfig.Configuration, typeof(BasicAuthInitialize));

            // Create the Autofac container
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }
    }
}
