using System;
using Steeltoe.Messaging.RabbitMQ.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging.RabbitMQ.Host;

namespace ConsoleSendReceive
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostBuilder = RabbitMQHost.CreateDefaultBuilder(args);

            hostBuilder.ConfigureServices((hostbuilderContext, services) => {
                services.AddLogging(b =>
                {
                    b.SetMinimumLevel(LogLevel.Information);
                    b.AddDebug();
                    b.AddConsole();
                });

                // Add queue to be declared
                services.AddRabbitQueue(new Queue("myqueue"));
            });

            using (var host = hostBuilder.Start())
            {
                var admin = host.Services.GetRabbitAdmin();
                var template = host.Services.GetRabbitTemplate();

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
                }
            }
        }
    }
}
