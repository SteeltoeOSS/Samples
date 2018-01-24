using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Discovery.Eureka;

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
            var configuration = builder.Build();

            // Setup logging
            var factory = new LoggerFactory();
            factory.AddConsole(configuration.GetSection("Logging"));

            // Build Eureka clients config from configuration
            var clientConfig = new EurekaClientConfig();
            ConfigurationBinder.Bind(configuration.GetSection("eureka:client"), clientConfig);

            // Create the Eureka client, start fetching registry in background thread
            var client = new DiscoveryClient(clientConfig, null, factory);

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
