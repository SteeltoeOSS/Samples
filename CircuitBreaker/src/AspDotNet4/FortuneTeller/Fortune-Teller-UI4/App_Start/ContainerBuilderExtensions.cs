using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pivotal.Discovery.Client;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.CircuitBreaker.Hystrix.Metric.Consumer;
using Steeltoe.CircuitBreaker.Hystrix.MetricsStream;
using Steeltoe.CircuitBreaker.Hystrix.Strategy;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Hystrix;
using Steeltoe.CloudFoundry.Connector.Services;
using System;

namespace FortuneTellerUI4
{
    public static class ContainerBuilderExtensions
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

        public static void RegisterHystrixCommand<TService, TImplementation>(this ContainerBuilder container, string groupKey, IConfiguration config)
                where TService : class where TImplementation : class, TService
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (string.IsNullOrEmpty(groupKey))
            {
                throw new ArgumentNullException(nameof(groupKey));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            RegisterHystrixCommand<TService, TImplementation>(container, HystrixCommandGroupKeyDefault.AsKey(groupKey), config);
        }

        public static void RegisterHystrixCommand<TService, TImplementation>(this ContainerBuilder container, IHystrixCommandGroupKey groupKey, IConfiguration config)
            where TService : class where TImplementation : class, TService
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (groupKey == null)
            {
                throw new ArgumentNullException(nameof(groupKey));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var strategy = HystrixPlugins.OptionsStrategy;
            var dynOpts = strategy.GetDynamicOptions(config);

            var commandKey = HystrixCommandKeyDefault.AsKey(typeof(TImplementation).Name);

            IHystrixCommandOptions opts = new HystrixCommandOptions(commandKey, null, dynOpts)
            {
                GroupKey = groupKey
            };
            container.RegisterType<TImplementation>().As<TService>().WithParameter(new TypedParameter(typeof(IHystrixCommandOptions), opts)).InstancePerDependency();
        }
        private static string[] rabbitAssemblies = new string[] { "RabbitMQ.Client" };
        private static string[] rabbitTypeNames = new string[] { "RabbitMQ.Client.ConnectionFactory" };
        public static void RegisterHystrixMetricsStream(this ContainerBuilder container, IConfiguration config)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            Type rabbitFactory = ConnectorHelpers.FindType(rabbitAssemblies, rabbitTypeNames);
            if (rabbitFactory == null)
            {
                throw new ConnectorException("Unable to find ConnectionFactory, are you missing RabbitMQ assembly");
            }

            HystrixRabbitServiceInfo info = config.GetSingletonServiceInfo<HystrixRabbitServiceInfo>();
            HystrixProviderConnectorOptions hystrixConfig = new HystrixProviderConnectorOptions(config);
            HystrixProviderConnectorFactory factory = new HystrixProviderConnectorFactory(info, hystrixConfig, rabbitFactory);
            container.Register(c => (HystrixConnectionFactory)factory.Create(null)).SingleInstance();
            container.RegisterInstance(HystrixDashboardStream.GetInstance()).SingleInstance();
            container.RegisterType<HystrixMetricsStreamPublisher>().SingleInstance();

        }


    }
}