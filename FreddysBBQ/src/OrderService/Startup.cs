using Common.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Models;
using Steeltoe.Connector.MySql.EFCore;
using Steeltoe.Discovery.Client;
using Steeltoe.Security.Authentication.CloudFoundry;

namespace OrderService
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
            services.AddHttpContextAccessor();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddCloudFoundryJwtBearer(Configuration);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Orders", policy => policy.RequireClaim("scope", "order.me"));
                options.AddPolicy("AdminOrders", policy => policy.RequireClaim("scope", "order.admin"));
            });

            services.AddDiscoveryClient(Configuration);

            services.AddDbContext<OrderContext>(options => options.UseMySql(Configuration));

            services.AddSingleton<IMenuService, MenuService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SampleData.InitializeOrderDatabase(app.ApplicationServices);
        }
    }
}
