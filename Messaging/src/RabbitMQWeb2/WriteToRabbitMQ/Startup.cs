using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Steeltoe.Connector.RabbitMQ;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;

namespace WriteToRabbitMQ
{
    public class Startup
    {
        public const string RECEIVE_AND_CONVERT_QUEUE = "steeltoe_message_queue";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRabbitMQConnection(Configuration);

            // Add Steeltoe Rabbit services, use default .NET serialization
            //services.AddRabbitServices();

            // Add Steeltoe Rabbit services, use JSON serialization
            services.AddRabbitServices(true);

            // Add Steeltoe RabbitAdmin services to get queues declared
            services.AddRabbitAdmin();

            // Add Steeltoe RabbitTemplate for sending/receiving
            services.AddRabbitTemplate();

            // Add a queue to the message container that the rabbit admin will discover and declare at startup
            services.AddRabbitQueue(new Queue(RECEIVE_AND_CONVERT_QUEUE));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WriteToRabbitMQ", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WriteToRabbitMQ"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
