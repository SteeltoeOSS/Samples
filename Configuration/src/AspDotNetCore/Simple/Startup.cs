using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Simple.Model;

using Steeltoe.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace Simple
{
    //
    // NOTE: This sample application assumes a running Spring Cloud Config Server is started
    //       with repository data for application named: foo, & profile: development
    //
    //       The easiest way to get that to happen is clone the spring-cloud-config
    //       repo and run the config-server.
    //          eg. git clone https://github.com/spring-cloud/spring-cloud-config.git
    //              cd spring-cloud-config\spring-cloud-config-server
    //              mvn spring-boot:run
    //

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()

                // Adds the Spring Cloud Configuration Server as a configuration source.
                // The settings used in contacting the Server will be picked up from
                // appsettings.json, and then overriden from any environment variables. 
                // Defaults will be used for any settings not present in either of those sources.
                // See ConfigServerClientSettings for defaults. 
                .AddConfigServer(env);

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Optional: Adds IConfigurationRoot as a service and also configures IOption<ConfigServerClientSettingsOptions> 
            // Performs:
            //      services.AddOptions();
            //      services.Configure<ConfigServerClientSettingsOptions>(config);
            //      services.AddSingleton<IConfigurationRoot>(config);
            services.AddConfigServer(Configuration);

            // Add framework services.
            services.AddMvc();

            // Add the configuration data returned from the Spring Cloud Config Server as IOption<>
            services.Configure<ConfigServerData>(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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
        }

    }
}
