using FortuneTellerService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Health;

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
            services.AddHealthActuator(Configuration);
            services.AddDbContext<FortuneContext>(options => options.UseInMemoryDatabase("Fortunes"), ServiceLifetime.Singleton);

            services.AddSingleton<IFortuneRepository, FortuneRepository>();

            // Add framework services.
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapDefaultControllerRoute();
                endpoints.Map<HealthEndpoint>();
            });

            SampleData.InitializeFortunesAsync(app.ApplicationServices).Wait();
            HealthStartupFilter.InitializeAvailability(app.ApplicationServices);
        }

    }
}
