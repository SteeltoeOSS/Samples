using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicStoreUI.Models;
using Steeltoe.Discovery.Client;
using Steeltoe.Extensions.Configuration.ConfigServer;
using Steeltoe.Management.Endpoint;
using System;

namespace MusicStoreUI
{
    public class Program
    {
        private static IConfiguration configuration;

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            SeedDatabase(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webbuilder => webbuilder.UseStartup<Startup>())
                .ConfigureAppConfiguration(AddRemoteConfiguration)
                .AddAllActuators()
                .AddDiscoveryClient();


        private static void SeedDatabase(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            SampleData.InitializeAccountsDatabase(services, configuration);
            SampleData.BuildFallbackData();
        }

        private static Action<HostBuilderContext, IConfigurationBuilder> AddRemoteConfiguration =>
            (builderContext, configBuilder) =>
            {
                if (builderContext.HostingEnvironment.EnvironmentName.Contains("Azure"))
                {
                    var settings = configBuilder.Build();
                    configBuilder.AddAzureAppConfiguration(options => options.Connect(new Uri(settings["AppConfig:Endpoint"]), new ManagedIdentityCredential()));
                }
                else
                {
                    configBuilder.AddConfigServer(builderContext.HostingEnvironment.EnvironmentName, GetLoggerFactory());
                }
                configBuilder.AddEnvironmentVariables();
                configuration = configBuilder.Build();
            };

        private static ILoggerFactory GetLoggerFactory()
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
