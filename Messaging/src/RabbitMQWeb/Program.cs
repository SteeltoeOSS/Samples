using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using RabbitMQWeb;
using Steeltoe.Messaging.RabbitMQ.Host;

var builder = RabbitMQHost.CreateDefaultBuilder(args);
builder.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

var host = builder.Build();

await host.RunAsync();