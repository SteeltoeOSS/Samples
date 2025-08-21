using Console.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Contexts;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Host;
using Steeltoe.Messaging.RabbitMQ.Listener;

var builder = RabbitMQHost.CreateDefaultBuilder(args);

builder.ConfigureServices((_, services) =>
{
    // Add queue to be declared
    services.AddRabbitQueue(new Queue("myQueue"));

    services.AddRabbitDirecListenerContainer(serviceProvider =>
    {
        var context = serviceProvider.GetRequiredService<IApplicationContext>();
        var factory = serviceProvider.GetRequiredService<IRabbitListenerContainerFactory>();
        var logger = serviceProvider.GetRequiredService<ILogger<SampleMessageListener>>();
        var listener = new SampleMessageListener(logger);
        var endpoint = new SimpleRabbitListenerEndpoint(context);
        endpoint.SetQueueNames("myQueue");
        endpoint.MessageListener = listener;
        var container = factory.CreateListenerContainer(endpoint) as DirectMessageListenerContainer;
        container.ServiceName = "manualContainer";
        return container;
    });

    // Add a message sender
    services.AddSingleton<IHostedService, SampleRabbitSender>();
});

var host = builder.Build();

await host.RunAsync();