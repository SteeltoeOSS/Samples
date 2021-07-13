using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Steeltoe.Common.Options;
using Steeltoe.Connector.Redis;
using Steeltoe.Security.Authentication.CloudFoundry;
using Steeltoe.Security.DataProtection;
using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

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

            services.AddCloudFoundryContainerIdentity();
            services.AddHttpClient("default", (services, client) =>
            {
                var options = services.GetService<IOptions<CertificateOptions>>();
                var b64 = Convert.ToBase64String(options.Value.Certificate.Export(X509ContentType.Cert));
                client.DefaultRequestHeaders.Add("X-Forwarded-Client-Cert", b64);
            }).ConfigurePrimaryHttpMessageHandler((isp) => new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true });


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

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
