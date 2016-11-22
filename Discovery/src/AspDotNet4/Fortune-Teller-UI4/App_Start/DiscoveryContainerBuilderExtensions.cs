using Autofac;
using Microsoft.Extensions.Configuration;
using Pivotal.Discovery.Client;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;


namespace FortuneTellerUI4
{
    public static class DiscoveryContainerBuilderExtensions
    {
        public static void RegisterDiscoveryClient(this ContainerBuilder container, IConfigurationRoot config)
        {
            EurekaServiceInfo info = config.GetSingletonServiceInfo<EurekaServiceInfo>();
            DiscoveryOptions configOptions = new DiscoveryOptions(config)
            {
                ClientType = DiscoveryClientType.EUREKA
            };

            DiscoveryClientFactory factory = new DiscoveryClientFactory(info, configOptions);
            container.Register<IDiscoveryClient>(c => (IDiscoveryClient)factory.CreateClient()).SingleInstance();
        }

    }
}