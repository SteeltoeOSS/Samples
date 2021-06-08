using Microsoft.Extensions.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.Placeholder;
using Steeltoe.Management.Endpoint;
using Steeltoe.Stream.StreamHost;
using System.Threading.Tasks;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Microsoft.AspNetCore.Hosting;


namespace UsageLogger
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            await StreamHost.CreateDefaultBuilder<UsageLogger>(args)
                  .ConfigureWebHostDefaults(webhostBuilder => webhostBuilder.UseStartup<Startup>())
                  .AddCloudFoundryConfiguration()
                  .UseCloudHosting()
                  .AddPlaceholderResolver()
                  .AddAllActuators()
                  .RunConsoleAsync();
        }
    }
}
