using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Models;
using Steeltoe.Common.Hosting;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint;
using System;

namespace MySql
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // Insert couple rows into table
                    SampleData.InitializeMySqlData(services).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .AddCloudFoundryConfiguration()
                .AddAllActuators()
                .UseStartup<Startup>()
                .UseCloudHosting()
                .Build();
        }
    }
}