using Owin;
using Steeltoe.Security.Authentication.CloudFoundry;

namespace CloudFoundryJwtAuthentication
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCloudFoundryJwtBearerAuthentication(ApplicationConfig.Configuration);
        }
   }
}