using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.Placeholder;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamHost;
using System.Threading.Tasks;

namespace UsageLogger
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            await StreamHost.CreateDefaultBuilder<UsageLogger>(args)
                    .ConfigureLogging(builder => builder.AddConsole())
                    .AddPlaceholderResolver()
                    .AddHealthActuator()
                    .RunConsoleAsync();
        }
    }
}
