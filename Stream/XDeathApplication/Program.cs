using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Messaging.RabbitMQ.Exceptions;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamHost;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XDeathApplication
{
    [EnableBinding(typeof(ISink))]
    public class Program
    {
        static async Task Main(string[] args)
        {
              var host = await StreamHost.CreateDefaultBuilder<Program>(args)
             .StartAsync();

        }

        [StreamListener(ISink.INPUT)]
        public void Listen(string input, 
            [Header(Name ="x-death", Required = false)]
            IDictionary<string, object> death)
        {
            if (death != null && (long)death["count"] == 3L)
            {
                // giving up - don't send to DLX
                throw new ImmediateAcknowledgeException("Failed after 4 attempts");
            }
            throw new RabbitRejectAndDontRequeueException("failed");
        }

    }
  
}
