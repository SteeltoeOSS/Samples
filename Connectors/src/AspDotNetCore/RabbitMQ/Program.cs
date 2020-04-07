using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.CloudFoundry;

namespace RabbitMQ
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config => config.AddCloudFoundry())
                .AddCloudFoundryActuators()
                .UseStartup<Startup>()
                .UseCloudHosting()
                .Build();

            host.Run();
        }
    }
}
