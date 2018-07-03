using Autofac;
using ManagementSysWeb.App_Start;
using Steeltoe.CloudFoundry.ConnectorAutofac;
using Steeltoe.Common.Configuration.Autofac;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.EndpointAutofac;
using Steeltoe.Management.EndpointSysWeb;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ManagementSysWeb
{
    public class Global : HttpApplication
    {
        public static IContainer Container;

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            ApplicationConfig.Register("development");
            ApplicationConfig.ConfigureLogging();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();
            builder.RegisterCloudFoundryOptions(ApplicationConfig.Configuration);
            builder.RegisterConfiguration(ApplicationConfig.Configuration);
            builder.RegisterCloudFoundryModules(ApplicationConfig.Configuration);
            builder.RegisterMySqlConnection(ApplicationConfig.Configuration);

            Container = builder.Build();
        }

        public override void Init()
        {
            // modules can be activated before or after base.Init(), 
            // just not during Application_Start or you'll get null references
            var modules = Container.Resolve<IEnumerable<IHttpModule>>();
            CloudFoundryModules.ActivateCloudFoundryModules(this, modules);
            base.Init();
        }
    }
}