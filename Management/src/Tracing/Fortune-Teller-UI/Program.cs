using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Discovery.Client;
using Steeltoe.Management.CloudFoundry;
using Steeltoe.Common.Hosting;

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
                    .AddCloudFoundryConfiguration()
                    .UseCloudHosting(5555)
                    .AddCloudFoundryActuators()
                    .AddDiscoveryClient()
                    .UseStartup<Startup>()
                    .Build();
    }
}
