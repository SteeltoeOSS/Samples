using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Messaging.RabbitMQ.Exceptions;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Extensions;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamHost;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Steeltoe.Common.Lifecycle;
using Steeltoe.Stream.Binder.Rabbit;
using Microsoft.Extensions.Options;
using Steeltoe.Stream.Binder.Rabbit.Config;
using System;

namespace XDeathApplication
{
    /// <summary>
    /// TODO: Doesn't work yet, Make it work 
    /// </summary>
    [EnableBinding(typeof(ISink))]
    public class Program
    {
        static async Task Main(string[] args)
        {

            var host = await StreamHost.CreateDefaultBuilder<Program>(args)
              .ConfigureServices((context, services) =>
              {
                  services.Configure<RabbitBindingsOptions>(context.Configuration.GetSection(RabbitBindingsOptions.PREFIX));

                  services.AddLogging(builder =>
                  {
                      builder.AddDebug();
                      builder.AddConsole();
                  });
              }).StartAsync();

            var bindingsOptions  = host.Services.GetService<IOptionsMonitor<RabbitBindingsOptions>>();
            var foo = bindingsOptions.CurrentValue;
            Console.WriteLine("AutoBindDlq" + foo.Default.Consumer.AutoBindDlq);
            Console.WriteLine("PREFIX " + RabbitBindingsOptions.PREFIX);

        }

        [StreamListener(ISink.INPUT)]
        public void Listen(string input, 
            [Header(Name ="x-death", Required = false)]
            ArrayList death)
        {
            var deathMaps = death?.Cast<IDictionary<string, object>>();
            if (deathMaps != null && deathMaps.Any(dm=> (long)dm["count"] == 3L))
            {
                // giving up - don't send to DLX
                throw new ImmediateAcknowledgeException("Failed after 4 attempts");
            }
            throw new RabbitRejectAndDontRequeueException("failed");
        }

    }
  
}
