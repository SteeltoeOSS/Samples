using Autofac;
using Autofac.Integration.Mvc;
using FortuneTellerUI4.Services;
using Pivotal.Discovery.Client;
using Steeltoe.CircuitBreaker.Hystrix.MetricsStream;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FortuneTellerUI4
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IDiscoveryClient _client;
        private HystrixMetricsStreamPublisher _publisher;

        protected void Application_Start()
        {
            var services = @"
{
      'p-service-registry': [
        {
                'credentials': {
                    'uri': 'https://eureka-a3f6ffcd-185e-43f7-a169-d1f58d8084ab.apps.testcloud.com',
            'client_secret': 'AZS8VHj9nf3q',
            'client_id': 'p-service-registry-84ca6c3b-963b-4eeb-ad63-3092146f0550',
            'access_token_uri': 'https://p-spring-cloud-services.uaa.system.testcloud.com/oauth/token'
                },
          'syslog_drain_url': null,
          'volume_mounts': [],
          'label': 'p-service-registry',
          'provider': null,
          'plan': 'standard',
          'name': 'myDiscoveryService',
          'tags': [
            'eureka',
            'discovery',
            'registry',
            'spring-cloud'
          ]
    }
      ],
      'p-circuit-breaker-dashboard': [
        {
          'credentials': {
            'stream': 'https://turbine-001ea2f2-96ab-45ff-b1b9-f7738a677f27.apps.testcloud.com',
            'amqp': {
              'http_api_uris': [
                'https://298f4565-b3fc-475d-9797-894143172238:q9r7n1ffp02it4ih3o68m89ll0@pivotal-rabbitmq.system.testcloud.com/api/'
              ],
              'ssl': false,
              'dashboard_url': 'https://pivotal-rabbitmq.system.testcloud.com/#/login/298f4565-b3fc-475d-9797-894143172238/q9r7n1ffp02it4ih3o68m89ll0',
              'password': 'q9r7n1ffp02it4ih3o68m89ll0',
              'protocols': {
                'amqp': {
                  'vhost': '62f3a9e1-eb14-4fd5-9323-8e5b5974f61a',
                  'username': '298f4565-b3fc-475d-9797-894143172238',
                  'password': 'q9r7n1ffp02it4ih3o68m89ll0',
                  'port': 5672,
                  'host': '192.168.1.57',
                  'hosts': [
                    '192.168.1.57'
                  ],
                  'ssl': false,
                  'uri': 'amqp://298f4565-b3fc-475d-9797-894143172238:q9r7n1ffp02it4ih3o68m89ll0@192.168.1.57:5672/62f3a9e1-eb14-4fd5-9323-8e5b5974f61a',
                  'uris': [
                    'amqp://298f4565-b3fc-475d-9797-894143172238:q9r7n1ffp02it4ih3o68m89ll0@192.168.1.57:5672/62f3a9e1-eb14-4fd5-9323-8e5b5974f61a'
                  ]
},
                'management': {
                  'path': '/api/',
                  'ssl': false,
                  'hosts': [
                    '192.168.1.57'
                  ],
                  'password': 'q9r7n1ffp02it4ih3o68m89ll0',
                  'username': '298f4565-b3fc-475d-9797-894143172238',
                  'port': 15672,
                  'host': '192.168.1.57',
                  'uri': 'http://298f4565-b3fc-475d-9797-894143172238:q9r7n1ffp02it4ih3o68m89ll0@192.168.1.57:15672/api/',
                  'uris': [
                    'http://298f4565-b3fc-475d-9797-894143172238:q9r7n1ffp02it4ih3o68m89ll0@192.168.1.57:15672/api/'
                  ]
                }
              },
              'username': '298f4565-b3fc-475d-9797-894143172238',
              'hostname': '192.168.1.57',
              'hostnames': [
                '192.168.1.57'
              ],
              'vhost': '62f3a9e1-eb14-4fd5-9323-8e5b5974f61a',
              'http_api_uri': 'https://298f4565-b3fc-475d-9797-894143172238:q9r7n1ffp02it4ih3o68m89ll0@pivotal-rabbitmq.system.testcloud.com/api/',
              'uri': 'amqp://298f4565-b3fc-475d-9797-894143172238:q9r7n1ffp02it4ih3o68m89ll0@192.168.1.57/62f3a9e1-eb14-4fd5-9323-8e5b5974f61a',
              'uris': [
                'amqp://298f4565-b3fc-475d-9797-894143172238:q9r7n1ffp02it4ih3o68m89ll0@192.168.1.57/62f3a9e1-eb14-4fd5-9323-8e5b5974f61a'
              ]
            },
            'dashboard': 'https://hystrix-001ea2f2-96ab-45ff-b1b9-f7738a677f27.apps.testcloud.com'
          },
          'syslog_drain_url': null,
          'volume_mounts': [],
          'label': 'p-circuit-breaker-dashboard',
          'provider': null,
          'plan': 'standard',
          'name': 'myHystrixService',
          'tags': [
            'circuit-breaker',
            'hystrix-amqp',
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
        'mem': 512,
        'disk': 1024
            },
      'application_name': 'fortuneUI',
      'application_uris': [
        'fortuneui.apps.testcloud.com'
      ],
      'name': 'fortuneUI',
      'space_name': 'test',
      'space_id': '54af9d15-2f18-453b-a533-f0c9e6522c97',
      'uris': [
        'fortuneui.apps.testcloud.com'
      ],
      'users': null,
      'application_id': 'da22082d-2103-4e05-baa3-94ff0456320c',
      'version': '58297625-74cf-4423-9e61-5f36450698b4',
      'application_version': '58297625-74cf-4423-9e61-5f36450698b4'
    }
 ";
            Environment.SetEnvironmentVariable("VCAP_SERVICES", services);
            Environment.SetEnvironmentVariable("VCAP_APPLICATION", app);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ServerConfig.RegisterConfig("development");

            var builder = new ContainerBuilder();

            // Register all the controllers with Autofac
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register IDiscoveryClient, etc.
            builder.RegisterDiscoveryClient(ServerConfig.Configuration); 

            // Register FortuneService Hystrix command
            builder.RegisterHystrixCommand<IFortuneService,FortuneService>("fortuneService", ServerConfig.Configuration);

            // Register Hystrix Metrics/Monitoring stream
            builder.RegisterHystrixMetricsStream(ServerConfig.Configuration);

            // Create the Autofac container
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Start the Discovery client background thread
            _client = container.Resolve<IDiscoveryClient>();

            // Start the Hystrix Metrics stream 
            _publisher = container.Resolve<HystrixMetricsStreamPublisher>();

        }
    }
}
