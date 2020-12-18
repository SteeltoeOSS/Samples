using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Discovery.Client;
using Steeltoe.Extensions.Configuration.CloudFoundry;

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
                    .UseCloudHosting(5000)
                    .AddCloudFoundryConfiguration()
                    .AddServiceDiscovery()
                    .UseStartup<Startup>()
                    .Build();
    }
}
