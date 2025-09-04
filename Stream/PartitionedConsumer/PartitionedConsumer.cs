using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Messaging.RabbitMQ;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamHost;
using System;
using System.Threading.Tasks;

namespace PartitionedConsumer;

[EnableBinding(typeof(ISink))]
public class PartitionedConsumer
{
    static async Task Main(string[] args)
    {
        var host = StreamHost
            .CreateDefaultBuilder<PartitionedConsumer>(args)
            .Build();
        await host.RunAsync();
    }


    [StreamListener(ISink.INPUT)]
    public void Listen([Payload] string input, [Header(RabbitMessageHeaders.CONSUMER_QUEUE)] string queue)
    {
        Console.WriteLine(input +" received from queue " + queue);
    }
}