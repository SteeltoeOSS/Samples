using Owin;
using Steeltoe.Security.Authentication.CloudFoundry.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudFoundrySingleSignon
{
	public partial class Startup
	{
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            //app.UseOpenIDConnect(new OpenIDConnectOptions {  });
        }
	}
}