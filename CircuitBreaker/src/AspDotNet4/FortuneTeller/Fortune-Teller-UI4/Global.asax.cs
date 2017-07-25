using Autofac;
using Autofac.Integration.Mvc;
using FortuneTellerUI4.Services;
using Pivotal.Discovery.Client;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FortuneTellerUI4
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IDiscoveryClient _client;

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

            // Register IDiscoveryClient, etc.
            builder.RegisterDiscoveryClient(ServerConfig.Configuration); 

            // Register FortuneService Hystrix command
            builder.RegisterHystrixCommand<IFortuneService,FortuneService>("fortuneService", ServerConfig.Configuration);

            // Register Hystrix Metrics/Monitoring stream
            //builder.RegisterHystrixMetricsStream(ServerConfig.Configuration);

            // Create the Autofac container
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Start the Discovery client background thread
            _client = container.Resolve<IDiscoveryClient>();
        }
    }
}
