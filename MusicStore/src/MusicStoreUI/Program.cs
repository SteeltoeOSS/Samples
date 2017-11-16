using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MusicStoreUI.Models;
using Pivotal.Extensions.Configuration.ConfigServer;
using Steeltoe.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace MusicStoreUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration config = null;

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(GetServerUrls(args))
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, configBuilder) =>
                {
                    var env = builderContext.HostingEnvironment;
                    configBuilder.SetBasePath(env.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                        .AddEnvironmentVariables()
                        .AddConfigServer(env.EnvironmentName);
                    config = configBuilder.Build();
                })
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddConfiguration(context.Configuration.GetSection("Logging"));
                    builder.AddDynamicConsole();
                })
                .Build();

            SeedDatabase(host, config);

            host.Run();
        }

        private static void SeedDatabase(IWebHost host, IConfiguration config)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    SampleData.InitializeAccountsDatabase(services, config);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
        }

        private static string[] GetServerUrls(string[] args)
        {
            List<string> urls = new List<string>();
            for (int i = 0; i < args.Length; i++)
            {
                if ("--server.urls".Equals(args[i], StringComparison.OrdinalIgnoreCase))
                {
                    urls.Add(args[i + 1]);
                }
            }
            return urls.ToArray();
        }
    }
}
