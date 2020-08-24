using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Security.Authentication.CloudFoundry;

namespace CloudFoundrySingleSignon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(cfg => cfg.AddCloudFoundryContainerIdentity("a8fef16f-94c0-49e3-aa0b-ced7c3da6229", "122b942a-d7b9-4839-b26e-836654b9785f"))
                .UseCloudHosting(8080, 8081)
                .AddCloudFoundryConfiguration()
                .UseStartup<Startup>()
                .Build();
    }
}
