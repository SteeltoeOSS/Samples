using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Stream.StreamHost;
using System.Threading.Tasks;

namespace EFCore;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = CreateHostBuilder(args);
        builder.ConfigureServices((context, services) =>
        {
            var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextPool<FooContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddDebug();
                loggingBuilder.AddConsole();
            });
            // Add Rabbit template
            services.AddRabbitTemplate();
            services.AddHostedService<MyRabbitSender>();
        });
        var host = builder.Build();
        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        StreamHost.CreateDefaultBuilder<BindableChannels>(args);
}