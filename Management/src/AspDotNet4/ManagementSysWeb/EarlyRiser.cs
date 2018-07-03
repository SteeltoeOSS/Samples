using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using ManagementSysWeb;
using ManagementSysWeb.App_Start;
using Steeltoe.Common.Configuration.Autofac;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.EndpointAutofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: PreApplicationStartMethod(typeof(EarlyRiser), "Initialize")]
namespace ManagementSysWeb
{
    public class EarlyRiser
    {
        public static void Initialize()
        {
            // Code that runs on application startup
            ApplicationConfig.Register("development");
            ApplicationConfig.ConfigureLogging();

            var builder = new ContainerBuilder();
            builder.RegisterCloudFoundryOptions(ApplicationConfig.Configuration);
            builder.RegisterConfiguration(ApplicationConfig.Configuration);
            builder.UseCloudFoundryModules(ApplicationConfig.Configuration);
            var container = builder.Build();

            var csl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => csl);
        }
    }
}