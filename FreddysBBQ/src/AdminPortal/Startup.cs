using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pivotal.Discovery.Client;
using Steeltoe.Security.Authentication.CloudFoundry;

namespace AdminPortal
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
            services.Configure<Branding>(Configuration.GetSection("Branding"));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CloudFoundryDefaults.AuthenticationScheme;
                })
                .AddCookie((options) =>
                {
                    options.AccessDeniedPath = new PathString("/Home/AccessDenied");
                })
                .AddCloudFoundryOAuth(Configuration);

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

            app.UseAuthentication();

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
