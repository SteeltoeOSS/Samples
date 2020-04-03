using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Common.Hosting;
using Steeltoe.Discovery.Client;
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
                   // .AddCloudFoundry()
                    .UseCloudHosting(5000)
                    .AddCloudFoundryActuators()
                    .AddServiceDiscovery()
                    .UseStartup<Startup>()
                    .Build();
    }
}
