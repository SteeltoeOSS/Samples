using Console.Common;
using EndpointRegistration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Host;

var builder = RabbitMQHost.CreateDefaultBuilder(args);

builder.ConfigureServices((_, services) =>
{
    // Add queue to be declared
    services.AddRabbitQueue(new Queue("myQueue"));

    // Add a configurer to configure listener endpoint
    services.AddSingleton<IRabbitListenerConfigurer, MyRabbitEndpointConfigurer>();

    // Add a message sender
    services.AddSingleton<IHostedService, SampleRabbitSender>();
});

var host = builder.Build();

await host.RunAsync();