using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShoppingCartService.Models;
using Steeltoe.Extensions.Configuration.ConfigServer;
using Steeltoe.Extensions.Logging;
using System;

namespace ShoppingCartService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            SeedDatabase(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webbuilder =>
                {
                    webbuilder
                        .UseStartup<Startup>()
                        .UseCloudFoundryHosting(6000);
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
                })
                .ConfigureLogging((context, builder) =>
                {
                    builder.ClearProviders();
                    builder.AddDynamicConsole();
                });

        private static void SeedDatabase(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                SampleData.InitializeShoppingCartDatabase(services);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }
    }
}
