using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint;

namespace Redis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder(args)
                .AddCloudFoundryConfiguration()
                .AddAllActuators()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
