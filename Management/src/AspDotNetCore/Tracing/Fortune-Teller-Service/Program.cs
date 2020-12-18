using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Discovery.Client;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.CloudFoundry;

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
                    .AddCloudFoundryConfiguration()
                    .AddCloudFoundryActuators()
                    .AddServiceDiscovery()
                    .UseCloudHosting(5000)
                    .UseStartup<Startup>()
                    .Build();
    }
}
