using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SimpleCloudFoundry4
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var services = @"
{
      'p-config-server': [
        {
                'credentials': {
                    'uri': 'https://config-0f8cd99e-8750-4928-bb5f-0a785427c34e.apps.testcloud.com',
            'client_secret': '3KYvJhwtj7ry',
            'client_id': 'p-config-server-d03f27c9-8027-48ae-9856-a9705f9e3c45',
            'access_token_uri': 'https://p-spring-cloud-services.uaa.system.testcloud.com/oauth/token'
                },
          'syslog_drain_url': null,
          'volume_mounts': [],
          'label': 'p-config-server',
          'provider': null,
          'plan': 'standard',
          'name': 'myConfigServer',
          'tags': [
            'configuration',
            'spring-cloud'
          ]
    }
      ]
    }
";
            var app = @"
{
      'cf_api': 'https://api.system.testcloud.com',
            'limits': {
                'fds': 16384,
        'mem': 1024,
        'disk': 1024
            },
      'application_name': 'foo',
      'application_uris': [
        'foo.apps.testcloud.com'
      ],
      'name': 'foo',
      'space_name': 'test',
      'space_id': '54af9d15-2f18-453b-a533-f0c9e6522c97',
      'uris': [
        'foo.apps.testcloud.com'
      ],
      'users': null,
      'application_id': '9cee5764-7925-4090-aa88-e100ff3374ad',
      'version': 'f22b28cc-e3ed-436a-a4bc-af576e0d5398',
      'application_version': 'f22b28cc-e3ed-436a-a4bc-af576e0d5398'
    }
";
            Environment.SetEnvironmentVariable("VCAP_SERVICES", services);
            Environment.SetEnvironmentVariable("VCAP_APPLICATION", app);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ServerConfig.RegisterConfig("development");
        }
    }
}
