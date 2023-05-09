using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Host;

namespace Receiver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RabbitMQHost.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Add the rabbit listener
                    services.AddSingleton<Tut3Receiver>();
                    services.AddRabbitListeners<Tut3Receiver>();
                })
                .Build()
                .Run();
        }
    }
}