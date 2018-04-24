
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace RedisDataProtectionKeyStore
{
    public class Program
    {

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                    .UseCloudFoundryHosting()
                    .AddCloudFoundry()
                    .UseStartup<Startup>()
                    .Build();

    }
}
