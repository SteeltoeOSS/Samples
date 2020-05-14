using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            var rabbitSection = Configuration.GetSection(RabbitOptions.PREFIX);
            services.Configure<RabbitOptions>(rabbitSection);
            services.AddRabbitServices();
            services.AddRabbitAdmin();
            services.AddRabbitTemplate();
            services.AddRabbitQueue(new AnonymousQueue(RabbitListenerService.INFERRED_FOO_QUEUE));
            services.AddRabbitQueue(new AnonymousQueue(RabbitListenerService.INFERRED_BAR_QUEUE));
            services.AddRabbitQueue(new Queue(RECEIVE_AND_CONVERT_QUEUE));
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
