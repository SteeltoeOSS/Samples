using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.CloudFoundry;

namespace RabbitMQ
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder(args)
                .AddCloudFoundry()
                .AddCloudFoundryActuators()
                .UseStartup<Startup>()
                .UseCloudFoundryHosting()
                .Build();

            host.Run();
        }
    }
}
