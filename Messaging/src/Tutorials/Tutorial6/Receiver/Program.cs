using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Host;

namespace Receiver
{
    internal class Program
    {
        internal const string RPCExchangeName = "tut.rpc";
        static void Main(string[] args)
        {
            RabbitMQHost.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Add the rabbit listener
                    services.AddSingleton<Tut6Receiver>();
                    services.AddRabbitListeners<Tut6Receiver>();
                })
                .Build()
                .Run();
        }
    }
}