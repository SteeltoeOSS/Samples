using Console.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Host;

var builder = RabbitMQHost.CreateDefaultBuilder(args);
builder.ConfigureServices((_, services) =>
{
    // Add queue to be declared
    services.AddRabbitQueue(new Queue("myQueue"));

    services.AddRabbitDirecListenerContainer("manualContainer", (serviceProvider, container) =>
    {
        var logger = serviceProvider.GetRequiredService<ILogger<SampleMessageListener>>();
        container.ApplicationContext = serviceProvider.GetApplicationContext();
        container.SetQueueNames("myQueue");
        container.MessageListener = new SampleMessageListener(logger);
    });

    // Add a message sender
    services.AddSingleton<IHostedService, SampleRabbitSender>();
});

var host = builder.Build();

await host.RunAsync();