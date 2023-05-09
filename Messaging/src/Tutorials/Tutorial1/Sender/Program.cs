using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Host;

namespace Sender
{
    public class Program
    {
        // The name of the queue that will be created
        internal const string QueueName = "hello";
        public static void Main(string[] args)
        {
            RabbitMQHost.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    // Add queue to service container to be declared
                    services.AddRabbitQueue(new Queue(QueueName));

                    services.AddHostedService<Tut1Sender>();
                })
                .Build()
                .Run();
        }
    }
}