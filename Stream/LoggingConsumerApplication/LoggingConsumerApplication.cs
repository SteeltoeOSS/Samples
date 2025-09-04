using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamHost;
using System;
using System.Threading.Tasks;

namespace LoggingConsumerApplication;

[EnableBinding(typeof(ISink))]
public class LoggingConsumerApplication
{
    static async Task Main(string[] args)
    {
        var host = StreamHost.CreateDefaultBuilder<LoggingConsumerApplication>(args)
            .ConfigureServices((context, services) =>
            {
                services.AddLogging(builder =>
                {
                    builder.AddDebug();
                    builder.AddConsole();
                });
            });

        await host.RunConsoleAsync();
    }

    [StreamListener(ISink.INPUT)]
    public void Handle(Person person)
    {
        Console.WriteLine("Received: " + person);
    }
}

public class Person
{
    public string Name { get; set; }

    public override string ToString()
    {
        return Name;
    }
}