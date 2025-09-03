using Microsoft.Extensions.Hosting;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamHost;
using System;
using System.Threading.Tasks;

namespace TestPocoWithAnnotatedArguments
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host = StreamHost
                .CreateDefaultBuilder<CatsAndDogs>(args)
                .Build();
            await host.RunAsync();
        }

        [EnableBinding(typeof(IProcessor))]
        public class CatsAndDogs
        {

            [StreamListener(ISink.INPUT, "Headers['type']=='Dog'")]
            public void Handle(Dog dog)
            {
                Console.WriteLine("Dog says:" + dog.Bark);
            }

            [StreamListener(ISink.INPUT, "Headers['type']=='Cat'")]
            public void Handle(Cat cat)
            {
                Console.WriteLine("Cat says:" + cat.Meow);
            }
        }
    }
}
