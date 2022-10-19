using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonitorRabbitMQ;
using Steeltoe.Connector.RabbitMQ;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using System.Runtime.CompilerServices;

namespace WriteToRabbitMQ
{
    public static class RabbitMQConfiguration
    {

        public const string RECEIVE_AND_CONVERT_QUEUE = "steeltoe_message_queue";
        public static void ConfigureRabbitMQ(this WebApplicationBuilder builder)
        {
            var services = builder.Services;
            services.AddRabbitMQConnection(builder.Configuration);

            // Add Steeltoe Rabbit services, use default .NET serialization
            //services.AddRabbitServices();

            // Add Steeltoe Rabbit services, use JSON serialization
            services.AddRabbitServices(true);

            // Add Steeltoe RabbitAdmin services to get queues declared
            services.AddRabbitAdmin();

            // Add a queue to the message container that the rabbit admin will discover and declare at startup
            services.AddRabbitQueue(new Queue(RECEIVE_AND_CONVERT_QUEUE));

            // Add singleton that will process incoming messages
            services.AddSingleton<RabbitListenerService>();

            // Tell steeltoe about singleton so it can wire up queues with methods to process queues
            services.AddRabbitListeners<RabbitListenerService>();
        }
    }
}
