namespace Microsoft.eShopOnContainers.Services.Marketing.API
{
    using AspNetCore.Hosting;
    using Microsoft.AspNetCore;
    using Microsoft.eShopOnContainers.Services.Marketing.API.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
  using Steeltoe.Extensions.Configuration.CloudFoundry;
  using Steeltoe.Extensions.Logging;
  using System.IO;

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
            .MigrateDbContext<MarketingContext>((context, services) =>
            {
                var logger = services.GetService<ILogger<MarketingContextSeed>>();

                new MarketingContextSeed()
                    .SeedAsync(context,logger)
                    .Wait();

            }).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseCloudFoundryHosting()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseWebRoot("Pics")
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddEnvironmentVariables();
                    config.AddCloudFoundry();
                })
                .ConfigureLogging((hostingContext, builder) =>
                {
                    builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    builder.AddDynamicConsole();
                    builder.AddDebug();
                })
                .Build();
    }
}
