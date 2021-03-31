using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamsHost;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Basic
{
    [EnableBinding(typeof(IProcessor))]
    public class Program
    {
        static async Task Main(string[] args)
        {

            var host = StreamsHost.CreateDefaultBuilder<Program>(args)
              .ConfigureServices((context, services) =>
              {
                  services.AddLogging(builder =>
                  {
                      builder.AddDebug();
                      builder.AddConsole();
                  });
              }).Build();
            
            var config = host.Services.GetService<IConfiguration>();

            Console.WriteLine(JsonSerializer.Serialize(config.AsEnumerable().ToList()));

            await host.StartAsync();
        }

        [StreamListener(IProcessor.INPUT)]
        [SendTo(IProcessor.OUTPUT)]
        public string Handle(string inputVal)
        {
            return inputVal.ToUpper();
        }

    }
  
}
