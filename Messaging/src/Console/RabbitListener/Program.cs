using Console.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitListener;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Host;

var builder = RabbitMQHost.CreateDefaultBuilder(args);
builder.ConfigureServices((_, services) =>
{
    // Add queue to be declared
    services.AddRabbitQueue(new Queue("myQueue"));

    // Add the rabbit listener
    services.AddSingleton<MyRabbitListener>();
    services.AddRabbitListeners<MyRabbitListener>();

    services.AddSingleton<IHostedService, SampleRabbitSender>();
});

var host = builder.Build();

await host.RunAsync();