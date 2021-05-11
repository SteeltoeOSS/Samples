using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Binder;
using Steeltoe.Stream.StreamHost;
using System.Threading.Tasks;

namespace PolledConsumer
{
    [EnableBinding(typeof(IPolledConsumerBinding))]
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
    public interface IPolledConsumerBinding
    {
        [Input]
        IPollableMessageSource DestIn { get; }

        [Output]
        IMessageChannel DestOut { get; }
    }

}
