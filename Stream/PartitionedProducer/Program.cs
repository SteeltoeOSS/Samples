using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamHost;
using System.Threading.Tasks;

namespace PartitionedProducer
{
    [EnableBinding(typeof(ISource))]
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host = StreamHost
              .CreateDefaultBuilder<Program>(args)
              .ConfigureServices(svc => svc.AddHostedService<Worker>())
              .Build();
            await host.StartAsync();
        }
    }
}
