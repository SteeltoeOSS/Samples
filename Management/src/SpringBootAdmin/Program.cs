using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.EndpointCore;
namespace CloudFoundry
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IHost BuildWebHost(string[] args) =>
            Host.CreateDefaultBuilder(args)

                .ConfigureWebHost(configure => {
                    configure.AddCloudFoundry();
                    configure.UseStartup<Startup>().UseKestrel();
                })
           //    .UseCloudHosting(5000, 5002)
               .AddAllActuators()
               .Build();
    }
}
