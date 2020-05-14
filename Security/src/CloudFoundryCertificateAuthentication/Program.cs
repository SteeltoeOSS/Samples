using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Security.Authentication.CloudFoundry;

namespace CloudFoundryCertificateAuthentication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(cfg => cfg.AddCloudFoundryContainerIdentity("a8fef16f-94c0-49e3-aa0b-ced7c3da6229", "122b942a-d7b9-4839-b26e-836654b9785f"))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseCloudHosting(8084, 8085);
    }
}
