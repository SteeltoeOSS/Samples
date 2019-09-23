using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
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
            IWebHost host = null;
            try
            {
                host = CreateWebHostBuilder(args).Build();
            }
            catch (ArgumentException e)
            {
                if (e.Message.Equals("Discovery client type UNKNOWN, check configuration"))
                {
                    Array.Resize(ref args, args.Length + 1);
                    args[^1] = "DisableServiceDiscovery=true";
                    host = CreateWebHostBuilder(args).Build();
                }
            }

            SeedDatabase(host);
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
                WebHost.CreateDefaultBuilder(args)
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
                        configuration = configBuilder.Build();
                    })
                    .ConfigureLogging((context, builder) =>
                    {
                        builder.AddDynamicConsole();
                    })
                   // .UseCloudFoundryHosting(5555)
                    .UseStartup<Startup>();


        private static void SeedDatabase(IWebHost host)
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
