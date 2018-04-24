using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.Management.CloudFoundry;
using Steeltoe.Security.DataProtection.CredHub;
using Steeltoe.Security.DataProtection.CredHubCore;

namespace CredHubDemo
{
    public class Startup
    {
        ILoggerFactory logFactory;

        public Startup(IConfiguration configuration, ILoggerFactory logFactory)
        {
            Configuration = configuration;
            this.logFactory = logFactory;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCloudFoundryActuators(Configuration);
            services.Configure<CredHubOptions>(Configuration.GetSection("CredHubClient"));
            services.AddCredHubClient(Configuration, logFactory);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseCloudFoundryActuators();
        }
    }
}
