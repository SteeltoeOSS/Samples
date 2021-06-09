using Steeltoe.Common.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Management.Endpoint;
using System.Threading.Tasks;
using Steeltoe.Stream.StreamHost;
using Steeltoe.Extensions.Configuration.Placeholder;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace UsageSender
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            await StreamHost
            .CreateDefaultBuilder<UsageGenerator>(args)
            .ConfigureWebHostDefaults(webhostBuilder => webhostBuilder.UseStartup<Startup>())
            .AddCloudFoundryConfiguration()
            .UseCloudHosting()
            .AddPlaceholderResolver()
            .AddAllActuators()
            .ConfigureServices(svc => svc.AddHostedService<UsageGenerator>())
            .Build()
            .RunAsync();
        }

    }
}
