using System;
using Steeltoe.Messaging.RabbitMQ.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging.RabbitMQ.Host;

var builder = RabbitMQHost.CreateDefaultBuilder(args);

builder.ConfigureServices((_, services) =>
{
    services.AddLogging(b =>
    {
        b.SetMinimumLevel(LogLevel.Information);
        b.AddDebug();
        b.AddConsole();
    });

    // Add queue to be declared
    services.AddRabbitQueue(new Queue("myQueue"));
});

var host = builder.Build();

var admin = host.Services.GetRabbitAdmin();
var template = host.Services.GetRabbitTemplate();

try
{
    template.ConvertAndSend("myQueue", "foo");
    var foo = template.ReceiveAndConvert<string>("myQueue");
    Console.WriteLine($"Received message from queue: {foo}");
}
finally
{
    // Delete queue and shutdown container
    admin.DeleteQueue("myQueue");
}

await host.RunAsync();