using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Configuration.Placeholder;
using Steeltoe.Discovery.Client;
using Steeltoe.Management.Endpoint;

namespace FortuneTellerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildHost(args).Run();
        }

        public static IHost BuildHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(host =>
                {
                    host.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration(builder => builder.AddPlaceholderResolver())
                .AddCloudFoundryConfiguration()
                .AddAllActuators()
                .AddDiscoveryClient()
                .Build();
    }
}
