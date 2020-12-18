using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace Fetch
{
    class Program
    {
        static void Main(string[] args)
        {
            // Build application configuration
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Environment.CurrentDirectory);
            builder.AddJsonFile("appsettings.json");
            builder.AddCloudFoundryConfiguration();
            var configuration = builder.Build();

            // Setup logging
            var factory = new LoggerFactory();
            factory.AddConsole(configuration.GetSection("Logging"));

            // Build Eureka clients config from configuration
            var clientOpts = new EurekaClientOptions();
            ConfigurationBinder.Bind(configuration.GetSection("eureka:client"), clientOpts);

            // if a Cloud Foundry Service Binding is found, update config
            var si = configuration.GetServiceInfos<EurekaServiceInfo>();
            if (si.Any())
            {
                EurekaPostConfigurer.UpdateConfiguration(configuration, si.First(), clientOpts);
            }

            // Create the Eureka client, start fetching registry in background thread
            var client = new DiscoveryClient(clientOpts, null, factory);

            do
            {
                // Get what applications have been fetched
                var apps = client.Applications;

                // Try to find app with name "MyApp", it is registered in the Register console application
                var app = apps.GetRegisteredApplication("MyApp");
                if (app != null)
                {
                    // Print the instance info, and then exit loop
                    Console.WriteLine("Successfully fetched application: {0} ", app);
                    break;
                }

            } while (true);

            Console.ReadLine();
        }
    }
}
