using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicStoreUI.Models;
using Steeltoe.Extensions.Configuration.ConfigServer;
using Steeltoe.Extensions.Logging;
using System;

namespace MusicStoreUI
{
    public class Program
    {
        private static IConfiguration configuration;

        public static void Main(string[] args)
        {
            IHost host = null;
            try
            {
                host = CreateHostBuilder(args).Build();
            }
            catch (ArgumentException e)
            {
                if (e.Message.Equals("Discovery client type UNKNOWN, check configuration"))
                {
                    Array.Resize(ref args, args.Length + 1);
                    args[^1] = "DisableServiceDiscovery=true";
                    host = CreateHostBuilder(args).Build();
                }
            }

            SeedDatabase(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webbuilder =>
                {
                    webbuilder
                        .UseStartup<Startup>()
                        .UseCloudFoundryHosting(5555);
                })
                .ConfigureAppConfiguration((builderContext, configBuilder) =>
                {
                    if (builderContext.HostingEnvironment.EnvironmentName.Contains("Azure"))
                    {
                        var settings = configBuilder.Build();
                        configBuilder.AddAzureAppConfiguration(options => options.ConnectWithManagedIdentity(settings["AppConfig:Endpoint"]));
                    }
                    else
                    {
                        configBuilder.AddConfigServer(builderContext.HostingEnvironment.EnvironmentName);
                    }
                    configBuilder.AddEnvironmentVariables();
                    configuration = configBuilder.Build();
                })
                .ConfigureLogging((context, builder) =>
                {
                    builder.ClearProviders();
                    builder.AddDynamicConsole();
                });


        private static void SeedDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    SampleData.InitializeAccountsDatabase(services, configuration);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            SampleData.BuildFallbackData();
        }
    }
}
