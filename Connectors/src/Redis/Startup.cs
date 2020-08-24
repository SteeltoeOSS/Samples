using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Redis.Models;
using Steeltoe.Connector.Redis;

namespace Redis
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add the Redis distributed cache.

            // We are using the Steeltoe Redis Connector to pickup the CloudFoundry
            // Redis Service binding and use it to configure the underlying RedisCache
            // This adds a IDistributedCache to the container
            services.AddDistributedRedisCache(Configuration);

            // This works like the above, but adds a IConnectionMultiplexer to the container
            services.AddRedisConnectionMultiplexer(Configuration);

            // Add framework services.
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Add some data to the Redis cache
            SampleData.InitializeCache(app.ApplicationServices).Wait();
        }
    }
}
