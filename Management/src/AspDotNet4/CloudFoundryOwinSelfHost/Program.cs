using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudFoundryOwinSelfHost
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var HttpPort = Environment.GetEnvironmentVariable("PORT") ?? "9000";
            var baseAddress = $"http://*:{HttpPort}/";
            ApplicationConfig.Register("Development");
            ApplicationConfig.ConfigureLogging();

            // Start OWIN host 
            // NOTE: if you receive an Access Denied error here, you can either:
            //   1. run Visual Studio as adminstrator
            //   2. add an ACL -- from admin powershell prompt: netsh http add urlacl url="http://*:9000/" sddl="D:(A;;GX;;;S-1-1-0)"
            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine($"Server running at {baseAddress}");

                // Create an HttpCient and make a request to api/values to make sure we're up and running
                HttpClient client = new HttpClient();

                var response = await client.GetAsync($"http://localhost:{HttpPort}/api/values");

                Console.WriteLine(response);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                Console.ReadLine();
            }
        }
    }
}
