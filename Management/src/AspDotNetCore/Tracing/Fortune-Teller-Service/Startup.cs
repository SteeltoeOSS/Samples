using FortuneTellerService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Management.Exporter.Tracing;
using Steeltoe.Management.Tracing;

namespace FortuneTellerService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkInMemoryDatabase().AddDbContext<FortuneContext>(
                options => options.UseInMemoryDatabase("Fortunes"), ServiceLifetime.Singleton);

            services.AddSingleton<IFortuneRepository, FortuneRepository>();

            // Add Distributed tracing
            services.AddDistributedTracing(Configuration);

            // Export traces to Zipkin
            services.AddZipkinExporter(Configuration);

            // Add framework services.
#if NETCOREAPP3_0
            services.AddControllers();
#else
            services.AddMvc();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();

#if NETCOREAPP3_0
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
#else
            app.UseMvc();
#endif

            // Start up trace exporter
            app.UseTracingExporter();

            SampleData.InitializeFortunesAsync(app.ApplicationServices).Wait();
        }
    }
}
