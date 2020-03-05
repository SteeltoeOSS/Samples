using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.CloudFoundry;

namespace Redis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder(args)
                .AddCloudFoundry()
                .AddCloudFoundryActuators()
                .UseStartup<Startup>()
                .UseCloudHosting()
                .Build();

            host.Run();
        }
    }
}
