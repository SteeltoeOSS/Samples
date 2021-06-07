using Steeltoe.Common.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Configuration.Placeholder;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Extensions;
using Steeltoe.Stream.Messaging;
using Steeltoe.Management.Endpoint;
using Steeltoe.Messaging.Handler.Attributes;
using System;
using System.Threading.Tasks;


namespace CloudDataflowToUpperProcessor
{
    [EnableBinding(typeof(IProcessor))]
    public class Program
    {
        public static void Main(string[] args)
        {
            var vars = Environment.GetEnvironmentVariables();
            foreach (var ev in vars.Keys)
            {
                Console.WriteLine(ev + ": "+ vars[ev]);
            }            
            var host = CreateStreamHostBuilder(args).Build();
            var config = host.Services.GetService<IConfiguration>(); 
            Console.WriteLine(config.GetValue<string>("spring:cloud:stream:bindings:input:destination"));
            // Console.WriteLine("Configuration");
            Console.WriteLine(((IConfigurationRoot)config).GetDebugView());
            host.Run();
        }

        public static IHostBuilder CreateStreamHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .AddCloudFoundryConfiguration()
                .AddPlaceholderResolver()
                .ConfigureWebHostDefaults(webhostBuilder => webhostBuilder.UseStartup<Startup>())
                .UseCloudHosting()
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
