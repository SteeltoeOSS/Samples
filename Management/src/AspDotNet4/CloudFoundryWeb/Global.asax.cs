using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CloudFoundryWeb
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ApplicationConfig.Configure("development");
            ApplicationConfig.ConfigureLogging();

            ManagementConfig.ConfigureManagementActuators(
                ApplicationConfig.Configuration,
                ApplicationConfig.LoggerProvider,
                GlobalConfiguration.Configuration.Services.GetApiExplorer(),
                ApplicationConfig.LoggerFactory);

            // Uncomment if you want to enable metrics exporting to Cloud Foundry metrics exporter, it's not enabled by default
            // Also see ManagementConfig.ConfigureManagementActuators() for more configuration needs
            // ManagementConfig.ConfigureMetricsExporter(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory);

            ManagementConfig.Start();
        }
        protected void Application_Stop()
        {
            ManagementConfig.Stop();
        }
    }
}
