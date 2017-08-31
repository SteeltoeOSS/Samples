using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;


namespace FortuneTellerUI4
{
    public static class DiscoveryContainerBuilderExtensions
    {
        public static void RegisterLoggingFactory(this ContainerBuilder container, ILoggerFactory factory)
        {
            container.RegisterInstance<ILoggerFactory>(factory).SingleInstance();

        }

        public static void RegisterDiscoveryClient(this ContainerBuilder container, IConfigurationRoot config, ILoggerFactory logFactory)
        {
            EurekaServiceInfo info = config.GetSingletonServiceInfo<EurekaServiceInfo>();
            DiscoveryOptions configOptions = new DiscoveryOptions(config)
            {
                ClientType = DiscoveryClientType.EUREKA
            };

            DiscoveryClientFactory factory = new DiscoveryClientFactory(info, configOptions);
            container.Register<IDiscoveryClient>(c => (IDiscoveryClient)factory.CreateClient(null, logFactory)).SingleInstance();
        }

    }
}