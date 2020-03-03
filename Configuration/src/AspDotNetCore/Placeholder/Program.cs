using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Extensions.Configuration.Placeholder;

namespace Placeholder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)

                // Add Steeltoe Placeholder resolver to apps configuration providers
                .AddPlaceholderResolver()
                .UseStartup<Startup>();
    }
}
