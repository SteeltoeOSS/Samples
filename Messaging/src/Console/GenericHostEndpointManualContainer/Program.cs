using System;
using ConsoleGenericHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Contexts;
using Steeltoe.Common.Lifecycle;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Connection;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Listener;

namespace ConsoleSendReceive
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Add core services
                services.AddRabbitHostingServices();
                services.AddRabbitDefaultMessageConverter();
                services.AddRabbitConnectionFactory();
                services.AddRabbitListenerContainerFactory();

                // Add Rabbit admin for auto queue declare
                services.AddRabbitAdmin();

                // Add Rabbit template for MyRabbitSender
                services.AddRabbitTemplate();

                // Add queue to be declared
                services.AddRabbitQueue(new Queue("myqueue"));


                services.AddRabbitDirecListenerContainer((p) =>
                {
                    var context = p.GetRequiredService<IApplicationContext>();
                    var factory = p.GetRequiredService<IRabbitListenerContainerFactory>();
                    var logFactory = p.GetRequiredService<ILoggerFactory>();
                    var listener = new MyMessageListener(logFactory.CreateLogger<MyMessageListener>());
                    var endpoint = new SimpleRabbitListenerEndpoint(context);
                    endpoint.SetQueueNames("myqueue");
                    endpoint.MessageListener = listener;
                    var container = factory.CreateListenerContainer(endpoint) as DirectMessageListenerContainer;
                    container.ServiceName = "manualContainer";
                    return container;
                });

                // Add a message sender
                services.AddSingleton<IHostedService, MyRabbitSender>();

            });
    }
}
