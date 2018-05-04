using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CloudFoundryJwtAuthentication.Startup))]

namespace CloudFoundryJwtAuthentication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
