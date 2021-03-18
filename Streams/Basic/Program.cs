using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.Converter;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamsHost;
using System;
using System.Threading.Tasks;

namespace Basic
{
    [EnableBinding(typeof(IProcessor))]
    public class Program
    {
        static async Task Main(string[] args)
        {

            await StreamsHost.CreateDefaultBuilder<Program>(args)
              .ConfigureServices((context, services) =>
              {
                  services.AddLogging(builder =>
                  {
                      builder.AddDebug();
                      builder.AddConsole();
                  });
              }).Build().StartAsync();

        }
        [StreamListener(IProcessor.INPUT)]
        [SendTo(IProcessor.OUTPUT)]
        public string Handle(string inputVal)
        {
            return inputVal.ToUpper();
        }

    }
  
}
