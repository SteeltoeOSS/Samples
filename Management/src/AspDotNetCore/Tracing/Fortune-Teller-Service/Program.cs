using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Logging;
using Microsoft.Extensions.Logging;
namespace FortuneTellerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                    .UseCloudFoundryHosting(5000)
                    .AddCloudFoundry()
                    .ConfigureLogging((builderContext, loggingBuilder) =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.AddConfiguration(builderContext.Configuration.GetSection("Logging"));
                        loggingBuilder.AddDynamicConsole();
                    })
                    .UseStartup<Startup>()
                    .Build();

    }
     
}
