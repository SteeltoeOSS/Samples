using Autofac;
using Autofac.Integration.WebApi;
using FortuneTellerService4.Models;
using Pivotal.Discovery.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace FortuneTellerService4
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IDiscoveryClient _client;

        protected void Application_Start()
        {


            GlobalConfiguration.Configure(WebApiConfig.Register);

            var config = GlobalConfiguration.Configuration;

            // Build application configuration
            ServerConfig.RegisterConfig("development");

            // Create IOC container builder
            var builder = new ContainerBuilder();

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register IDiscoveryClient, etc.
            builder.RegisterDiscoveryClient(ServerConfig.Configuration);

            // Initialize and Register FortuneContext
            builder.RegisterInstance(SampleData.InitializeFortunes()).SingleInstance();

            // Register FortuneRepository
            builder.RegisterType<FortuneRepository>().As<IFortuneRepository>().SingleInstance();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Start the Discovery client background thread
            _client = container.Resolve<IDiscoveryClient>();
        }

        protected void Application_End()
        {
            // Unregister current app with Service Discovery server
            _client.ShutdownAsync();
        }
    }
}
