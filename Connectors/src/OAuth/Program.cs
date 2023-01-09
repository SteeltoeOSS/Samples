using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint;

namespace OAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .AddCloudFoundryConfiguration()
                .AddAllActuators()
                .UseStartup<Startup>()
                .Build();
        }
    }
}
