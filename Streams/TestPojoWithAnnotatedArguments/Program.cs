using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Expression.Internal;
using Steeltoe.Common.Expression.Internal.Spring.Standard;
using Steeltoe.Common.Expression.Internal.Spring.Support;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamsHost;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace VoteHandler
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host = StreamsHost
              .CreateDefaultBuilder<CatsAndDogs>(args)
              .Build();
            await host.StartAsync();
        }

        [EnableBinding(typeof(IProcessor))]
        public class CatsAndDogs
        {
         
            [StreamListener(ISink.INPUT, "Headers['type']=='Dog'")]
            public void Handle(Dog dog)
            {
                Console.WriteLine("Dog says:"+ dog.Bark);
            }

            [StreamListener(ISink.INPUT, "Headers['type']=='Cat'")]
            public void Handle(Cat cat)
            {
                Console.WriteLine("Cat says:" +cat.Meow);
            }
        }

    }

}
