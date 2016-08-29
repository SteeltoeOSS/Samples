using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;

namespace RabbitDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://0.0.0.0:8080")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup(typeof(Program).GetTypeInfo().Assembly.GetName().Name)
                .Build();

            host.Run();
        }
    }
}
