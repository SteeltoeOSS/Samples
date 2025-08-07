using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Connector;
using Steeltoe.Connector.MySql;

namespace CloudFoundry
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
            // Add microsoft community health checks 
            var cm = new ConnectionStringManager(Configuration);
            var connectionString = cm.Get<MySqlConnectionInfo>().ConnectionString;
            services.AddHealthChecks().AddMySql(connectionString); 

            // Add in a MySql connection (this method also adds an IHealthContributor for it)
            services.AddMySqlConnection(Configuration); //will use microsoft health check instead of steeltoe health check

            services.AddHealthChecksUI().AddMySqlStorage(connectionString);

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
            
            // Optionally use Microsoft health middleware for MsftHealth Checks, listening at path /Health
            app.UseHealthChecks("/Health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            //Optionally use health checks ui at /healthchecks-ui
            app.UseHealthChecksUI();

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
