using System;
using Steeltoe.Messaging.RabbitMQ.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ConsoleSendReceive
{
    class Program
    {
        private static ServiceProvider container;

        static void Main(string[] args)
        {
            container = CreateServiceContainer();
            var admin = container.GetRabbitAdmin();
            var template = container.GetRabbitTemplate();
            try
            {
                template.ConvertAndSend("myqueue", "foo");
                var foo = template.ReceiveAndConvert<string>("myqueue");
                Console.WriteLine(foo);
            }
            finally
            {
                // Delete queue and shutdown container
                admin.DeleteQueue("myqueue");
                container.Dispose();
            }
        }

        private static ServiceProvider CreateServiceContainer()
        {
            var services = new ServiceCollection();

            // Add some logging
            services.AddLogging(b =>
            {
                b.SetMinimumLevel(LogLevel.Information);
                b.AddDebug();
                b.AddConsole();
            });

            // Add any configuration as needed
            var config = new ConfigurationBuilder().Build();

            // Configure any rabbit options from configuration
            var rabbitSection = config.GetSection(RabbitOptions.PREFIX);
            services.Configure<RabbitOptions>(rabbitSection);
            services.AddSingleton<IConfiguration>(config);

            // Add Steeltoe Rabbit services
            services.AddRabbitServices();
            services.AddRabbitAdmin();
            services.AddRabbitTemplate();

            // Add queue to be declared
            services.AddRabbitQueue(new Queue("myqueue"));

            // Build container and start
            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<IHostedService>().StartAsync(default).Wait();
            return provider;
        }
    }
}
