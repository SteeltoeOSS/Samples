using Autofac;
using Autofac.Integration.Mvc;
using CredHubClient.App_Start;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Steeltoe.Common.Logging.Autofac;
using Steeltoe.Common.Options.Autofac;
using Steeltoe.Security.DataProtection.CredHub;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace CredHubClient
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static string OriginalServices;

        protected void Application_Start()
        {
            MockVcapServices();
            OriginalServices = Environment.GetEnvironmentVariable("VCAP_SERVICES");

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ApplicationConfig.RegisterConfig("development");
            ApplicationConfig.ConfigureLogging();

            var builder = new ContainerBuilder();
            builder.RegisterOptions();
            builder.RegisterLogging(ApplicationConfig.Configuration);

            // Register all the controllers with Autofac
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // configure CredHub client
            var credHubOptions = ApplicationConfig.Configuration.GetSection("CredHubClient").Get<CredHubOptions>();
            credHubOptions.Validate();
            builder.RegisterInstance(credHubOptions);
            var credHubClient = Steeltoe.Security.DataProtection.CredHub.CredHubClient.CreateUAAClientAsync(credHubOptions).GetAwaiter().GetResult();
            builder.RegisterInstance(credHubClient).As<ICredHubClient>().SingleInstance();

            // Create the Autofac container
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private void MockVcapServices()
        {
            // forcefully setup credentials that need to be interpolated
            string services = @"
                { 
                    ""p-demo-resource"": [
                    {
                    ""credentials"": {
                        ""credhub-ref"": ""((/credhubdemo-config-server/credentials))""
                    },
                    ""label"": ""p-config-server"",
                    ""name"": ""config-server"",
                    ""plan"": ""standard"",
                    ""provider"": null,
                    ""syslog_drain_url"": null,
                    ""tags"": [
                        ""configuration"",
                        ""spring-cloud""
                    ],
                    ""volume_mounts"": []
                    }]
                }";

            // comment out this line if you have your own service instance to test interpolation with
            Environment.SetEnvironmentVariable("VCAP_SERVICES", services);
        }
    }
}
