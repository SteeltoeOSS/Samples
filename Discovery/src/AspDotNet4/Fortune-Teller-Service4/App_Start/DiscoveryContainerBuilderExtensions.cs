using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pivotal.Discovery.Client;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FortuneTellerService4
{
    public static class DiscoveryContainerBuilderExtensions
    {
        public static void RegisterLoggingFactory(this ContainerBuilder container, ILoggerFactory factory)
        {
            container.RegisterInstance<ILoggerFactory>(factory).SingleInstance();
        }

        public static void RegisterDiscoveryClient(this ContainerBuilder container, IConfigurationRoot config, ILoggerFactory loggerFactory)
        {
            EurekaServiceInfo info = config.GetSingletonServiceInfo<EurekaServiceInfo>();
            DiscoveryOptions configOptions = new DiscoveryOptions(config)
            {
                ClientType = DiscoveryClientType.EUREKA
            };

            DiscoveryClientFactory factory = new DiscoveryClientFactory(info, configOptions);
            container.Register<IDiscoveryClient>(c => (IDiscoveryClient)factory.CreateClient(null, loggerFactory)).SingleInstance();
        }

    }
}