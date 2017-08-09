using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Management.Endpoint.CloudFoundry;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.Endpoint.Info;
using Steeltoe.Management.Endpoint.Loggers;
using Steeltoe.Management.Endpoint.Trace;

namespace CloudFoundry
{
    public static class ManagementActuatorExtensions
    {
        public static void AddCloudFoundryActuators(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors();
            services.AddCloudFoundryActuator(config);
            services.AddInfoActuator(config);
            services.AddHealthActuator(config);
            services.AddLoggersActuator(config);
            services.AddTraceActuator(config);
        }
        public static void UseCloudFoundryActuators(this IApplicationBuilder app)
        {
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                .WithMethods("GET", "POST")
                .WithHeaders("Authorization", "X-Cf-App-Instance", "Content-Type");
            });

            app.UseCloudFoundrySecurity();
            app.UseCloudFoundryActuator();
            app.UseInfoActuator();
            app.UseHealthActuator();
            app.UseLoggersActuator();
            app.UseTraceActuator();
        }
    }
}
