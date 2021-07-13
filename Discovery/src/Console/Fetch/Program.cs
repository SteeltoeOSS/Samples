using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System.Threading.Tasks;

namespace Fetch
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config => config.AddCloudFoundry())
                .ConfigureServices(services =>
                {
                    services.AddServiceDiscovery(client => client.UseEureka());
                    services.AddHostedService<ConsoleFetchService>();
                })
                .RunConsoleAsync();
        }
    }
}
