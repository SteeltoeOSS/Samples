using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint;

namespace SpringBootAdmin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IHost BuildWebHost(string[] args) =>
            Host.CreateDefaultBuilder(args)

                .ConfigureWebHost(configure =>
                {
                    configure.AddCloudFoundryConfiguration();
                    configure.UseStartup<Startup>().UseKestrel();
                    configure.UseUrls("http://host.docker.internal:5000");
                })
               .AddAllActuators(endpoints => endpoints.RequireAuthorization("actuators.read"))
               .Build();
    }
}
