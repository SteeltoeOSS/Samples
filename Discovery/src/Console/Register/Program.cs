using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Discovery.Eureka;

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
            var configuration = builder.Build();


            // Setup logging
            var factory = new LoggerFactory();
            factory.AddConsole(configuration.GetSection("Logging"));


            // Build Eureka clients config from configuration
            var clientConfig = new EurekaClientConfig();
            ConfigurationBinder.Bind(configuration.GetSection("eureka:client"), clientConfig);

            // Build Eureka instance info config from configuration
            var instConfig = new EurekaInstanceConfig();
            ConfigurationBinder.Bind(configuration.GetSection("eureka:instance"), instConfig);

            // Initialize ApplicationManager with instance configuration
            ApplicationInfoManager.Instance.Initialize(instConfig, factory);

            // Create the Eureka client, the Application is registered and renewed with registry
            var client = new DiscoveryClient(clientConfig, null, factory);

            // Hang and keep renewing the registration
            Console.ReadLine();
        }
    }
}
