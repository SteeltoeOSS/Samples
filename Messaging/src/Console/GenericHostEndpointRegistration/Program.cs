using ConsoleGenericHost;
using GenericHostEndpointRegistration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Host;

namespace ConsoleSendReceive
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            RabbitMQHost.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Add queue to be declared
                    services.AddRabbitQueue(new Queue("myqueue"));

                    // Add a configurer to configure listener endpoint
                    services.AddSingleton<IRabbitListenerConfigurer, MyRabbitEndpointConfigurer>();

                    // Add a message sender
                    services.AddSingleton<IHostedService, MyRabbitSender>();
                });
    }
}
