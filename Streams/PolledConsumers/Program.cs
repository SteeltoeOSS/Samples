using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Expression.Internal;
using Steeltoe.Common.Expression.Internal.Spring.Standard;
using Steeltoe.Common.Expression.Internal.Spring.Support;
using Steeltoe.Messaging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Binder;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamHost;
using System;
using System.Linq;
using System.Text.Json;
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
