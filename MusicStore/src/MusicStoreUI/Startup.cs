
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pivotal.Extensions.Configuration;
using Pivotal.Discovery.Client;
using SteelToe.CloudFoundry.Connector.Redis;
using MusicStoreUI.Services;
using MusicStoreUI.Models;
using SteelToe.CloudFoundry.Connector.MySql.EF6;
using Microsoft.AspNet.Identity.EntityFramework;
using MusicStoreUI.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

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


            services.AddDbContext<AccountsContext>(Configuration);

            var builder = services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Cookies.ApplicationCookie.AccessDeniedPath = "/Home/AccessDenied";
            });

            builder.AddEntityFrameworkStores().AddDefaultTokenProviders();

       
            services.AddDiscoveryClient(Configuration);

            services.AddSingleton<IMusicStore, MusicStoreService>();
            services.AddSingleton<IShoppingCart, ShoppingCartService>();
            services.AddSingleton<IOrderProcessing, OrderProcessingService>();

            services.AddLogging();

            services.AddMvc();

            // Add memory cache services
            services.AddMemoryCache();

#if USE_REDIS_CACHE
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


            app.UseGoogleAuthentication(new GoogleOptions
            {
                ClientId = "995291875932-0rt7417v5baevqrno24kv332b7d6d30a.apps.googleusercontent.com",
                ClientSecret = "J_AT57H5KH_ItmMdu0r6PfXm"
            });

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
