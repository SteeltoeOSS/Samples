using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Logging;

namespace OrderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
                WebHost.CreateDefaultBuilder(args)
                        .UseCloudHosting()
                        .AddCloudFoundryConfiguration()
                        .ConfigureLogging((builderContext, loggingBuilder) =>
                        {
                            loggingBuilder.AddConfiguration(builderContext.Configuration.GetSection("Logging"));
                            loggingBuilder.AddDynamicConsole();
                        })
                        .UseStartup<Startup>()
                        .Build();
    }
}
