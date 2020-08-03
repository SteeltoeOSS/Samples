using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQWeb.Services;
using Steeltoe.Messaging.Rabbit.Config;
using Steeltoe.Messaging.Rabbit.Extensions;

namespace RabbitMQWeb
{
    public class Startup
    {
        public const string RECEIVE_AND_CONVERT_QUEUE = "sample1.receive.and.convert";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure any rabbit client values;
            var rabbitSection = Configuration.GetSection(RabbitOptions.PREFIX);
            services.Configure<RabbitOptions>(rabbitSection);

            // Add steeltoe rabbit services
            services.AddRabbitServices();
            
            // Add the steeltoe rabbit admin client... will be used to declare queues below
            services.AddRabbitAdmin();

            // Add some queues to the container that the rabbit admin will discover and declare at startup
            services.AddRabbitQueue(new Queue(RabbitListenerService.INFERRED_FOO_QUEUE));
            services.AddRabbitQueue(new Queue(RabbitListenerService.INFERRED_BAR_QUEUE));
            services.AddRabbitQueue(new Queue(RECEIVE_AND_CONVERT_QUEUE));

            // Add the rabbit client template used for send and receiving messages... used in RabbitTestController
            services.AddRabbitTemplate();

            // Add singleton that will process incoming messages
            services.AddSingleton<RabbitListenerService>();

            // Tell steeltoe about singleton so it can wire up queues with methods to process queues (i.e. RabbitListenerAttribute)
            services.AddRabbitListeners<RabbitListenerService>();

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
