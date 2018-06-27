using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.eShopOnContainers.Services.Identity.API.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Logging;
using System.IO;

namespace Microsoft.eShopOnContainers.Services.Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .MigrateDbContext<PersistedGrantDbContext>((_, __) => { })
                .MigrateDbContext<ApplicationDbContext>((context, services) =>
                {
                    var env = services.GetService<IHostingEnvironment>();
                    var logger = services.GetService<ILogger<ApplicationDbContextSeed>>();
                    var settings = services.GetService<IOptions<AppSettings>>();

                    new ApplicationDbContextSeed()
                        .SeedAsync(context, env, logger, settings)
                        .Wait();
                })
                .MigrateDbContext<ConfigurationDbContext>((context,services)=> 
                {
                    var configuration = services.GetService<IConfiguration>();

                    new ConfigurationDbContextSeed()
                        .SeedAsync(context, configuration)
                        .Wait();
                }).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                //.UseHealthChecks("/hc")es
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseCloudFoundryHosting()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddEnvironmentVariables();
                    // Add to configuration the Cloudfoundry VCAP settings
                    config.AddCloudFoundry();
                })
                .ConfigureLogging((hostingContext, builder) =>
                {
                    builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    builder.AddDebug();
                    builder.AddDynamicConsole();
                    
                })
                //.UseApplicationInsights()
                .Build();
    }
}

