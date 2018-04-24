using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CloudFoundrySingleSignon.Startup))]

namespace CloudFoundrySingleSignon
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
