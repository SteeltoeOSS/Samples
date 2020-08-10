using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Security.Authentication.CloudFoundry;
using Steeltoe.Management.Endpoint.Loggers;
using Steeltoe.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Steeltoe.Management.Endpoint;
using Microsoft.AspNetCore.Builder;
using Steeltoe.Management.CloudFoundry;

namespace SecureWithUAA
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
                    configure.ConfigureAppConfiguration(cfg => cfg.AddCloudFoundryContainerIdentity("a8fef16f-94c0-49e3-aa0b-ced7c3da6229", "122b942a-d7b9-4839-b26e-836654b9785f"));
                    configure.AddCloudFoundryConfiguration();
                    configure.UseStartup<Startup>().UseKestrel();
                })
               .UseCloudHosting(5000, 5002)
               .AddAllActuators(endpoints => endpoints.RequireAuthorization("actuators.read"))
               .Build();
    }
}
