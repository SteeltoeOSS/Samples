
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pivotal.Extensions.Configuration;
using Pivotal.Discovery.Client;

#if NET451 && USE_REDIS_CACHE
using Steeltoe.CloudFoundry.Connector.Redis;
#endif

using MusicStoreUI.Services;
using MusicStoreUI.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

#if NET451 && MYSQL
using Microsoft.AspNet.Identity.EntityFramework;
using MusicStoreUI.Identity;
using Steeltoe.CloudFoundry.Connector.MySql.EF6;
#endif

#if !NET451 || POSTGRES
using Steeltoe.CloudFoundry.Connector.PostgreSql.EFCore;
#endif

namespace MusicStoreUI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddConfigServer(env);
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

#if NET451 && MYSQL
            services.AddDbContext<AccountsContext>(Configuration);

            var builder = services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Cookies.ApplicationCookie.AccessDeniedPath = "/Home/AccessDenied";
            });

            builder.AddEntityFrameworkStores().AddDefaultTokenProviders();

#endif
#if !NET451 || POSTGRES
            services.AddDbContext<AccountsContext>(options => options.UseNpgsql(Configuration));
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                    {
                        options.Cookies.ApplicationCookie.AccessDeniedPath = "/Home/AccessDenied";
                    })
                    .AddEntityFrameworkStores<AccountsContext>()
                    .AddDefaultTokenProviders();
#endif
            services.AddDiscoveryClient(Configuration);

            services.AddSingleton<IMusicStore, MusicStoreService>();
            services.AddSingleton<IShoppingCart, ShoppingCartService>();
            services.AddSingleton<IOrderProcessing, OrderProcessingService>();

            services.AddLogging();

            services.AddMvc();

            // Add memory cache services
            services.AddMemoryCache();

#if NET451 && USE_REDIS_CACHE
            services.AddDistributedRedisCache(Configuration);
#else
            services.AddDistributedMemoryCache();
#endif

            // Add session related services.
            services.AddSession();

            // Configure Auth
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "ManageStore",
                    authBuilder =>
                    {
                        authBuilder.RequireClaim("ManageStore", "Allowed");
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            app.UseSession();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // Add cookie-based authentication to the request pipeline
            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}",
                    defaults: new { action = "Index" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseDiscoveryClient();

            SampleData.InitializeAccountsDatabaseAsync(app.ApplicationServices, Configuration).Wait();

        }
    }
}
