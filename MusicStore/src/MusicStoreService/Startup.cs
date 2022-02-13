using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicStore.Models;
using Steeltoe.Connector.MySql.EFCore;
using Steeltoe.Management.Tracing;

namespace MusicStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Steeltoe MySQL Connector
            //services.AddDbContext<MusicStoreContext>(options => options.UseSqlServer(Configuration));
            services.AddDbContext<MusicStoreContext>(options => options.UseMySql(Configuration));

            services.AddDistributedTracing();

            // Add Framework services
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
