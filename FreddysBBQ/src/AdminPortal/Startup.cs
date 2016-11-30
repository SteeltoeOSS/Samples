
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Common.Models;
using Pivotal.Extensions.Configuration;
using Pivotal.Discovery.Client;
using Steeltoe.Security.Authentication.CloudFoundry;
using Common.Services;
using Microsoft.AspNetCore.Http;

namespace AdminPortal
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddConfigServer(env)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Branding>(Configuration.GetSection("Branding"));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddCloudFoundryAuthentication(Configuration);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("MenuWrite", policy => policy.RequireClaim("scope", "menu.write"));
                options.AddPolicy("AdminOrders", policy => policy.RequireClaim("scope", "order.admin"));
            });

            services.AddDiscoveryClient(Configuration);

            services.AddSingleton<IMenuService, MenuService>();
            services.AddSingleton<IOrderService, OrderService>();

            // Add framework services.
            services.AddMvc();
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

            app.UseCloudFoundryAuthentication(new CloudFoundryOptions()
            {
                AccessDeniedPath = new PathString("/Home/AccessDenied")
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseDiscoveryClient();
        }
    }
}
