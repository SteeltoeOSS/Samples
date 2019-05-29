using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace Register
{
    class Program
    {
        static void Main(string[] args)
        {
            // Build application configuration
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Environment.CurrentDirectory);
            builder.AddJsonFile("appsettings.json");
            builder.AddCloudFoundry();
            var configuration = builder.Build();


            // Setup logging
            var factory = new LoggerFactory();
            factory.AddConsole(configuration.GetSection("Logging"));


            // Build Eureka clients config from configuration
            var clientOpts = new EurekaClientOptions();
            ConfigurationBinder.Bind(configuration.GetSection(EurekaClientOptions.EUREKA_CLIENT_CONFIGURATION_PREFIX), clientOpts);

            // Build Eureka instance info config from configuration
            var instOpts = new EurekaInstanceOptions();
            ConfigurationBinder.Bind(configuration.GetSection(EurekaInstanceOptions.EUREKA_INSTANCE_CONFIGURATION_PREFIX), instOpts);

            // if a Cloud Foundry Service Binding is found, update config
            var si = configuration.GetServiceInfos<EurekaServiceInfo>();
            if (si.Any())
            {
                EurekaPostConfigurer.UpdateConfiguration(configuration, si.First(), clientOpts);
                EurekaPostConfigurer.UpdateConfiguration(configuration, instOpts);
            }

            // Initialize ApplicationManager with instance configuration
            ApplicationInfoManager.Instance.Initialize(instOpts, factory);

            // Create the Eureka client, the Application is registered and renewed with registry
            var client = new DiscoveryClient(clientOpts, null, factory);

            // Hang and keep renewing the registration
            Console.ReadLine();
        }
    }
}
