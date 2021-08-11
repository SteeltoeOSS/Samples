using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Steeltoe.Actuators.Providers;
using Steeltoe.Actuators.Services;

namespace Steeltoe.Actuators
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
            services.AddControllersWithViews();

            services.AddDbContext<EmployeeDataContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DbConnection")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Steeltoe Actuators", Version = "v1" });
            });

            services.AddHttpContextAccessor();
            services.AddTransient<IActuatorLinkService, ActuatorLinkService>();

            services.AddHttpClient<ILogLevelService, LogLevelService>((provider, client) =>
            {
                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                var scheme = httpContextAccessor.HttpContext.Request.Scheme;
                var host = httpContextAccessor.HttpContext.Request.Host.ToUriComponent();
                client.BaseAddress = new System.Uri($"{scheme}://{host}");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, EmployeeDataContext employeeDataContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Page/Error");
                app.UseHsts();
            }

            // Run latest migrations if any
            employeeDataContext.Database.Migrate();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Steeltoe Actuators v1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Page}/{action=Index}/{id?}");
            });
        }
    }
}
