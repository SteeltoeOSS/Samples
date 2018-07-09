using Autofac;
using Autofac.Integration.Mvc;
using CloudFoundryOwin.App_Start;
using Steeltoe.CloudFoundry.ConnectorAutofac;
using Steeltoe.Common.Configuration.Autofac;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.EndpointAutofac;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CloudFoundryOwin
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ApplicationConfig.Register("development");
            ApplicationConfig.ConfigureLogging();

            var builder = new ContainerBuilder();

            // Register all the controllers with Autofac
            builder.RegisterControllers(typeof(Global).Assembly);
            builder.RegisterCloudFoundryOptions(ApplicationConfig.Configuration);
            builder.RegisterConfiguration(ApplicationConfig.Configuration);
            builder.RegisterMySqlConnection(ApplicationConfig.Configuration);
            builder.UseCloudFoundryMiddlewares(ApplicationConfig.Configuration);

            ApplicationConfig.Container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(ApplicationConfig.Container));
        }
    }
}