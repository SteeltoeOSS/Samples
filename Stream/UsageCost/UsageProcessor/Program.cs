using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Extensions.Configuration.Placeholder;
using Steeltoe.Management.Endpoint;
using Steeltoe.Stream.StreamHost;
using System.Threading.Tasks;

namespace UsageProcessor
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            await StreamHost.CreateDefaultBuilder<UsageProcessor>(args)
            .AddPlaceholderResolver()
            .AddHealthActuator()
            .RunConsoleAsync();
        }
    }
}
