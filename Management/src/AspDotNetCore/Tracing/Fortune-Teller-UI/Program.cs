using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Discovery.Client;
using Steeltoe.Management.CloudFoundry;

namespace Fortune_Teller_UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                    .AddCloudFoundry()
                    .AddCloudFoundryActuators()
                    .AddServiceDiscovery()
                    .UseStartup<Startup>()
                    .UseCloudFoundryHosting(5555)
                    .Build();
    }
}
