using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQWeb.Services;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;

namespace RabbitMQWeb;

public class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Add some queues to the container that the rabbit admin will discover and declare at startup
        services.AddRabbitQueue(new Queue(Queues.InferredRabbitQueue));
        services.AddRabbitQueue(new Queue(Queues.InferredLongEaredRabbitQueue));
        services.AddRabbitQueue(new Queue(Queues.SendReceiveRabbitQueue));
        var quorum = QueueBuilder.Durable(Queues.QuorumQueue).Quorum().DeliveryLimit(10).Build();
        services.AddRabbitQueue(quorum);


        // Add singleton that will process incoming messages
        services.AddSingleton<RabbitListener>();

        // Tell steeltoe about singleton so it can wire up queues with methods to process queues (i.e. RabbitListenerAttribute)
        services.AddRabbitListeners<RabbitListener>();

        services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}