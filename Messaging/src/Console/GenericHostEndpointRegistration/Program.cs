using System;
using ConsoleGenericHost;
using GenericHostEndpointRegistration;
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
                services.AddRabbitServices();

                // Add Rabbit admin for auto queue declare
                services.AddRabbitAdmin();

                // Add Rabbit template for MyRabbitSender
                services.AddRabbitTemplate();

                // Add queue to be declared
                services.AddRabbitQueue(new Queue("myqueue"));

                // Add a configurer to configure listener endpoint
                services.AddSingleton<IRabbitListenerConfigurer, MyRabbitEndpointConfigurer>();

                // Add a message sender
                services.AddSingleton<IHostedService, MyRabbitSender>();

            });
    }
}
