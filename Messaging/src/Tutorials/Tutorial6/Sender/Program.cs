using Steeltoe.Messaging.RabbitMQ.Host;
namespace Sender
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RabbitMQHost.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Tut6Sender>();
                })
                .Build().Run();
        }
    }
}