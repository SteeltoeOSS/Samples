using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Pivotal.Discovery.Eureka;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery.Eureka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FortuneTellerService4
{
    public static class DiscoveryConfig
    {
        public const string EUREKA_PREFIX = "eureka";

        public static IDiscoveryClient DiscoveryClient { get; internal set; }
        public static void Register(IConfiguration configuration, IDiscoveryLifecycle discoveryLifecycle)
        {
            IServiceInfo info = GetSingletonDiscoveryServiceInfo(configuration);
            DiscoveryClient = CreateDiscoveryClient(info, configuration, discoveryLifecycle);
        }

        private static IServiceInfo GetSingletonDiscoveryServiceInfo(IConfiguration config)
        {
            var eurekaInfos = config.GetServiceInfos<EurekaServiceInfo>();

            if (eurekaInfos.Count > 0)
            {
                if (eurekaInfos.Count != 1)
                {
                    throw new ConnectorException(string.Format("Multiple discovery service types bound to application."));
                }

                return eurekaInfos[0];
            }

            return null;
        }
  
        private static IDiscoveryClient CreateDiscoveryClient(IServiceInfo info, IConfiguration config, IDiscoveryLifecycle lifecycle)
        {
            var clientConfigsection = config.GetSection(EUREKA_PREFIX);

            int childCount = clientConfigsection.GetChildren().Count();
            if (childCount > 0)
            {
                EurekaServiceInfo einfo = info as EurekaServiceInfo;
                var clientSection = config.GetSection(EurekaClientOptions.EUREKA_CLIENT_CONFIGURATION_PREFIX);
                EurekaClientOptions clientOptions = new EurekaClientOptions();
                ConfigurationBinder.Bind(clientSection, clientOptions);
                if (einfo != null)
                {
                    PivotalEurekaConfigurer.UpdateConfiguration(config, einfo, clientOptions);
                }
        

                var instSection = config.GetSection(EurekaInstanceOptions.EUREKA_INSTANCE_CONFIGURATION_PREFIX);
                EurekaInstanceOptions instOptions = new EurekaInstanceOptions();
                ConfigurationBinder.Bind(instSection, instOptions);
                if (einfo != null)
                {
                    PivotalEurekaConfigurer.UpdateConfiguration(config, einfo, instOptions);
                }
                var manager = new EurekaApplicationInfoManager(new OptionsMonitorWrapper<EurekaInstanceOptions>(instOptions), LoggingConfig.LoggerFactory);

                return new PivotalEurekaDiscoveryClient(
                    new OptionsMonitorWrapper<EurekaClientOptions>(clientOptions),
                    new OptionsMonitorWrapper<EurekaInstanceOptions>(instOptions),
                    manager,
                    null,
                    LoggingConfig.LoggerFactory);
            }
            else
            {
                throw new ArgumentException("Unable to create Eureka client");
            }
        }

    }
    public class OptionsMonitorWrapper<T> : IOptionsMonitor<T>
    {
        private T Options { get; }
        public OptionsMonitorWrapper(T options)
        {
            Options = options;
        }

        public T CurrentValue => Options;
        public T Get(string name)
        {
            throw new NotImplementedException();
        }

        public IDisposable OnChange(Action<T, string> listener)
        {
            throw new NotImplementedException();
        }
    }
}