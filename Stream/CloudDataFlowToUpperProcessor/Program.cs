using Steeltoe.Common.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Extensions;
using Steeltoe.Stream.Messaging;
using Steeltoe.Management.Endpoint;
using Steeltoe.Messaging.Handler.Attributes;
using System;

namespace CloudDataflowToUpperProcessor
{
    [EnableBinding(typeof(IProcessor))]
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateStreamHostBuilder(args).Build();
            host.Run();
        }

        public static IHostBuilder CreateStreamHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webhostBuilder => webhostBuilder.UseStartup<Startup>())
                .UseCloudHosting()
                .AddCloudFoundryConfiguration()
                .AddAllActuators()
                .AddStreamServices<Program>();

        [StreamListener(IProcessor.INPUT)]
        [SendTo(IProcessor.OUTPUT)]
        public string Handle(string inputVal)
        {
            Console.WriteLine("converting " + inputVal);
            return inputVal.ToUpper();
        }
    }
}
