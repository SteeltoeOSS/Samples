using Autofac;
using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FortuneTellerOwinService4
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var HttpPort = Environment.GetEnvironmentVariable("PORT") ?? "5001";
            var baseAddress = $"http://*:{HttpPort}/";
            ApplicationConfig.Register("Development");

            // Start OWIN host 
            // NOTE: if you receive an Access Denied error here, you can either:
            //   1. run Visual Studio as adminstrator
            //   2. add an ACL -- from admin powershell prompt: netsh http add urlacl url="http://*:9000/" sddl="D:(A;;GX;;;S-1-1-0)"
            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine($"Server running at {baseAddress}");

                // Create an HttpCient and make a request to api/values to make sure we're up and running
                var client = new HttpClient();

                var response = await client.GetAsync($"http://localhost:{HttpPort}/api/fortunes");

                Console.WriteLine(response);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                Console.ReadLine();
            }
        }
    }
}