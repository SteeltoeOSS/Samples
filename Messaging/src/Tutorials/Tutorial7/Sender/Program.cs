using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Connection;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Host;
namespace Sender
{
    public class Program
    {
        internal const string QueueName = "hello";
        public static void Main(string[] args)
        {
            RabbitMQHost.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    // Add queue to service container to be declared
                    services.AddRabbitQueue(new Queue(QueueName));

                    services.AddRabbitConnectionFactory("publisherConfirmReturnsFactory", (p, ccf) =>
                    {
                        ccf.IsPublisherReturns = true;
                        ccf.PublisherConfirmType = CachingConnectionFactory.ConfirmType.CORRELATED;
                    });

                    services.AddRabbitTemplate("confirmReturnsTemplate", (p, template) =>
                    {
                        var ccf = p.GetRabbitConnectionFactory("publisherConfirmReturnsFactory");
                        template.ConnectionFactory = ccf;
                        template.Mandatory = true;
                    });

                    services.AddHostedService<Tut7Sender>();
                })
                .Build()
                .Run();
        }
    }
}