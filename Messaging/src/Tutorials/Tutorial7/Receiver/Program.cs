using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Host;
namespace Receiver
{
    internal class Program
    {
        internal const string QueueName = "hello";

        static void Main(string[] args)
        {
            RabbitMQHost.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Add queue to service container to be declared
                    services.AddRabbitQueue(new Queue(QueueName));

                    // Add the rabbit listener
                    services.AddSingleton<Tut7Receiver>();
                    services.AddRabbitListeners<Tut7Receiver>();
                })
                .Build()
                .Run();
        }
    }
}