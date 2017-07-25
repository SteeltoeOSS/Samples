using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fortune_Teller_UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = @"
{
      'p-service-registry': [
        {
                'credentials': {
                    'uri': 'https://eureka-20bc737d-a2e5-4bcb-a905-fc25a6d0599d.apps.testcloud.com',
            'client_secret': 'Bs0QvoEd7ybI',
            'client_id': 'p-service-registry-36f46870-09c5-40eb-815d-60a79757f38f',
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
            'stream': 'https://turbine-bd34f489-82d5-45be-a491-b9b9c0dbfd3c.apps.testcloud.com',
            'amqp': {
              'http_api_uris': [
                'https://593c0c0b-0ae2-4129-bb3e-289c56343340:n76e0lmt1a5rrtdbtni462glaj@pivotal-rabbitmq.system.testcloud.com/api/'
              ],
              'ssl': false,
              'dashboard_url': 'https://pivotal-rabbitmq.system.testcloud.com/#/login/593c0c0b-0ae2-4129-bb3e-289c56343340/n76e0lmt1a5rrtdbtni462glaj',
              'password': 'n76e0lmt1a5rrtdbtni462glaj',
              'protocols': {
                'amqp': {
                  'vhost': '77b38bde-9909-4d39-bcf0-7b060c07a30a',
                  'username': '593c0c0b-0ae2-4129-bb3e-289c56343340',
                  'password': 'n76e0lmt1a5rrtdbtni462glaj',
                  'port': 5672,
                  'host': '192.168.1.57',
                  'hosts': [
                    '192.168.1.57'
                  ],
                  'ssl': false,
                  'uri': 'amqp://593c0c0b-0ae2-4129-bb3e-289c56343340:n76e0lmt1a5rrtdbtni462glaj@192.168.1.57:5672/77b38bde-9909-4d39-bcf0-7b060c07a30a',
                  'uris': [
                    'amqp://593c0c0b-0ae2-4129-bb3e-289c56343340:n76e0lmt1a5rrtdbtni462glaj@192.168.1.57:5672/77b38bde-9909-4d39-bcf0-7b060c07a30a'
                  ]
},
                'management': {
                  'path': '/api/',
                  'ssl': false,
                  'hosts': [
                    '192.168.1.57'
                  ],
                  'password': 'n76e0lmt1a5rrtdbtni462glaj',
                  'username': '593c0c0b-0ae2-4129-bb3e-289c56343340',
                  'port': 15672,
                  'host': '192.168.1.57',
                  'uri': 'http://593c0c0b-0ae2-4129-bb3e-289c56343340:n76e0lmt1a5rrtdbtni462glaj@192.168.1.57:15672/api/',
                  'uris': [
                    'http://593c0c0b-0ae2-4129-bb3e-289c56343340:n76e0lmt1a5rrtdbtni462glaj@192.168.1.57:15672/api/'
                  ]
                }
              },
              'username': '593c0c0b-0ae2-4129-bb3e-289c56343340',
              'hostname': '192.168.1.57',
              'hostnames': [
                '192.168.1.57'
              ],
              'vhost': '77b38bde-9909-4d39-bcf0-7b060c07a30a',
              'http_api_uri': 'https://593c0c0b-0ae2-4129-bb3e-289c56343340:n76e0lmt1a5rrtdbtni462glaj@pivotal-rabbitmq.system.testcloud.com/api/',
              'uri': 'amqp://593c0c0b-0ae2-4129-bb3e-289c56343340:n76e0lmt1a5rrtdbtni462glaj@192.168.1.57/77b38bde-9909-4d39-bcf0-7b060c07a30a',
              'uris': [
                'amqp://593c0c0b-0ae2-4129-bb3e-289c56343340:n76e0lmt1a5rrtdbtni462glaj@192.168.1.57/77b38bde-9909-4d39-bcf0-7b060c07a30a'
              ]
            },
            'dashboard': 'https://hystrix-bd34f489-82d5-45be-a491-b9b9c0dbfd3c.apps.testcloud.com'
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
  }
";
            var application = @"
{
      'cf_api': 'https://api.system.testcloud.com',
        'limits': {
            'fds': 16384,
            'mem': 1024,
            'disk': 1024
        },
      'application_name': 'fortuneui',
      'application_uris': [
        'fortuneui.apps.testcloud.com'
      ],
      'name': 'fortuneui',
      'space_name': 'test',
      'space_id': '0495100a-4892-47d8-a902-1c7cf3d54450',
      'uris': [
        'fortuneui.apps.testcloud.com'
      ],
      'users': null,
      'application_id': '5aa2e837-4518-4df2-b34b-7b79213f21ff',
      'version': 'a87ed4c2-0de5-4fa3-b106-9c52a4d3a3e6',
      'application_version': 'a87ed4c2-0de5-4fa3-b106-9c52a4d3a3e6'
}
";
            Environment.SetEnvironmentVariable("VCAP_SERVICES", services);
            Environment.SetEnvironmentVariable("VCAP_APPLICATION", application);

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(GetServerUrls(args))
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

        private static string[] GetServerUrls(string[] args)
        {
            List<string> urls = new List<string>();
            for(int i = 0; i < args.Length; i++)
            {
                if ("--server.urls".Equals(args[i], StringComparison.OrdinalIgnoreCase))
                {
                    urls.Add(args[i + 1]);
                }
            }
            return urls.ToArray();
        }
    }
}
