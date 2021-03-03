using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Steeltoe.CloudFoundry.ConnectorAutofac;
using Steeltoe.Common.Configuration.Autofac;
using Steeltoe.Common.Options.Autofac;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.EndpointOwinAutofac;
using Steeltoe.Management.EndpointOwinAutofac.Actuators;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace CloudFoundryOwinAutofac
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ApplicationConfig.Register("development");
            ApplicationConfig.ConfigureLogging();

            var builder = new ContainerBuilder();

            // Register all the controllers with Autofac
            builder.RegisterControllers(typeof(Global).Assembly);
            builder.RegisterApiControllers(typeof(Global).Assembly);
            builder.RegisterCloudFoundryOptions(ApplicationConfig.Configuration);
            builder.RegisterConfiguration(ApplicationConfig.Configuration);
            builder.RegisterMySqlConnection(ApplicationConfig.Configuration);
            builder.RegisterCloudFoundryActuators(ApplicationConfig.Configuration);

            // workaround for https://github.com/SteeltoeOSS/Steeltoe/issues/460 -- can be removed after upgrade to 2.5.3
            builder.RegisterOptions();

            // Uncomment if you want to enable metrics actuator endpoint, it's not enabled by default
            // builder.RegisterMetricsActuator(ApplicationConfig.Configuration);

            // Uncomment if you want to enable exporting of metrics to PCF Metrics, it's not enabled by default
            // builder.RegisterMetricsForwarderExporter(ApplicationConfig.Configuration);

            var container = ApplicationConfig.Container = builder.Build();

            container.StartActuators();

            // Uncomment if you want to enable exporting of metrics to the Cloud Foundry metrics exporter, it's not enabled by default
            // container.StartMetricsExporter();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(ApplicationConfig.Container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}