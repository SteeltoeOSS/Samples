using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Discovery.Client;
using Steeltoe.Management.Endpoint;

namespace Fortune_Teller_UI
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
                .AddCloudFoundryConfiguration()
                .AddAllActuators()
                .AddDiscoveryClient()
                .Build();
    }
}
