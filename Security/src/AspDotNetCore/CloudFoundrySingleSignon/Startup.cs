using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
// using Steeltoe.CloudFoundry.Connector.Redis;
using Steeltoe.Security.Authentication.CloudFoundry;
// using Steeltoe.Security.DataProtection;

namespace CloudFoundrySingleSignon
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddOptions();

            services.AddAuthentication((options) =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CloudFoundryDefaults.AuthenticationScheme;
            })
            .AddCookie((options) =>
            {
                options.AccessDeniedPath = new PathString("/Home/AccessDenied");
            })
            .AddCloudFoundryOAuth(Configuration);
            //.AddCloudFoundryOpenIdConnect(Configuration);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("testgroup", policy => policy.RequireClaim("scope", "testgroup"));
                options.AddPolicy("testgroup1", policy => policy.RequireClaim("scope", "testgroup1"));
            });

            // Add Redis to allow scaling beyond a single instance
            // services.AddRedisConnectionMultiplexer(Configuration);
            // services.AddDataProtection()
            //     .PersistKeysToRedis()
            //     .SetApplicationName("fortuneui");

            // services.AddDistributedRedisCache(Configuration);
            // services.AddSession();

#if NETCOREAPP3_1 || NET5_0
            services.AddControllersWithViews();
#else
            services.AddMvc();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#if NETCOREAPP3_1 || NET5_0
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
#else
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
#endif
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

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });

#if NETCOREAPP3_1 || NET5_0
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
#else
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
#endif
        }
    }
}
