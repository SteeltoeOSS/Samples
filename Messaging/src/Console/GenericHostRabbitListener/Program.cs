using System;
using ConsoleGenericHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;

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

                // Add Rabbit admin
                services.AddRabbitAdmin();

                // Add Rabbit template
                services.AddRabbitTemplate();

                // Add queue to be declared
                services.AddRabbitQueue(new Queue("myqueue"));

                // Add the rabbit listener
                services.AddSingleton<MyRabbitListener>();
                services.AddRabbitListeners<MyRabbitListener>();

                services.AddSingleton<IHostedService, MyRabbitSender>();

            });
    }
}
