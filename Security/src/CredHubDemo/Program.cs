using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Logging;
using Steeltoe.Security.DataProtection.CredHubCore;
using System;
using System.IO;

namespace CredHubDemo
{
    public class Program
    {
        public static string OriginalServices;

        public static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            
            // forcefully setup credentials that need to be interpolated
            string services = @"
                { 
                    ""p-demo-resource"": [
                    {
                    ""credentials"": {
                        ""credhub-ref"": ""((/credhubdemo-config-server/credentials))""
                    },
                    ""label"": ""p-config-server"",
                    ""name"": ""config-server"",
                    ""plan"": ""standard"",
                    ""provider"": null,
                    ""syslog_drain_url"": null,
                    ""tags"": [
                        ""configuration"",
                        ""spring-cloud""
                    ],
                    ""volume_mounts"": []
                    }]
                }";

            // comment out this line if you have your own service instance to test interpolation with
            Environment.SetEnvironmentVariable("VCAP_SERVICES", services);
            OriginalServices = Environment.GetEnvironmentVariable("VCAP_SERVICES");

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseCloudFoundryHosting()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.SetBasePath(builderContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{builderContext.HostingEnvironment.EnvironmentName}.json", optional: true)
                        .AddCloudFoundry()
                        .AddEnvironmentVariables();
                })
                .ConfigureLogging((builderContext, loggingBuilder) =>
                {
                    loggingBuilder.AddConfiguration(builderContext.Configuration.GetSection("Logging"));
                    loggingBuilder.AddDynamicConsole();
                })
                .UseCredHubInterpolation(GetLoggerFactory())
                .Build();

            host.Run();
        }

        public static ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Trace));
            serviceCollection.AddLogging(builder => builder.AddConsole((opts) =>
            {
                opts.DisableColors = true;
            }));
            serviceCollection.AddLogging(builder => builder.AddDebug());
            return serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
        }
    }
}
