using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Discovery.Client;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint;

namespace FortuneTeller.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                    .AddCloudFoundryConfiguration()
                    .UseCloudHosting(5000)
                    .AddAllActuators()
                    .AddDiscoveryClient()
                    .UseStartup<Startup>()
                    .Build();
    }
}
