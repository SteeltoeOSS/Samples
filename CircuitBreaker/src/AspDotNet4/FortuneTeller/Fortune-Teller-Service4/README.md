# Fortune-Teller-Service - ASP.NET 4 WebApi Microservice

ASP.NET 4 WebApi sample app illustrating how to use [Spring Cloud Eureka Server](http://projects.spring.io/spring-cloud) for registering micro services  and [Spring Cloud Hystrix](http://cloud.spring.io/spring-cloud) for building resilient micro services applications. The Fortune-Teller-Service registers the fortuneService with the Eureka server upon startup and the Fortune-Teller-UI uses a Hystrix command with fallback ability when communicating with the Fortune service.

This sample also illustrates how to use the [Hystrix Dashboard](http://cloud.spring.io/spring-cloud) to gather status and metrics of the Hystrix command used in communications.

Note: This application is built using the Autofac IOC container.

## Pre-requisites - Running on CloudFoundry

1. Installed Pivotal CloudFoundry with Windows support
1. Installed Spring Cloud Services

## Setup Service Registry on CloudFoundry

You must first create an instance of the Service Registry service in a org/space.

1. cf target -o myorg -s development
1. cf create-service p-service-registry standard myDiscoveryService
1. Wait for service to be ready. (use `cf services` to check the status)

## Publish App & Push to CloudFoundry

1. Open Samples\CircuitBreaker\src\AspDotNet4\FortuneTeller.sln in Visual Studio.
1. Select Fortune-Teller-Service4 project in Solution Explorer.
1. Right-click and select Publish
1. Publish the App to a folder. (e.g. c:\publish)
1. cd publish_folder (e.g. cd c:\publish)
1. cf push

Windows Note: If you are using self-signed certificates you are likely to run into SSL certificate validation issues when pushing this app. You have two choices to fix this:

1. If you have created your own ROOT CA and from it created a certificate that you have installed in HAProxy/Ext LB, then you can install the ROOT CA on the windows cells and you would be good to go.
1. Disable certificate validation for the Spring Cloud Discovery Client.  You can do this by editing `appsettings.json` and add `eureka:client:validate_certificates=false`.

## What to expect - CloudFoundry

After building and running the app, you should see something like the following in the logs.

To see the logs as you startup and use the app: `cf logs fortuneservice`

You should see something like this during startup:

```text
2016-11-22T09:31:32.48-0700 [STG/0]      OUT Successfully created container
2016-11-22T09:31:32.49-0700 [STG/0]      OUT Downloading app package...
2016-11-22T09:31:35.02-0700 [STG/0]      OUT Staging...
2016-11-22T09:31:35.02-0700 [STG/0]      OUT Downloaded app package (5.2M)
2016-11-22T09:31:38.27-0700 [STG/0]      OUT Exit status 0
2016-11-22T09:31:38.27-0700 [STG/0]      OUT Staging complete
2016-11-22T09:31:38.27-0700 [STG/0]      OUT Uploading droplet, build artifacts cache...
2016-11-22T09:31:38.27-0700 [STG/0]      OUT Uploading build artifacts cache...
2016-11-22T09:31:38.27-0700 [STG/0]      OUT Uploading droplet...
2016-11-22T09:31:38.34-0700 [STG/0]      OUT Uploaded build artifacts cache (88B)
2016-11-22T09:31:43.62-0700 [STG/0]      OUT Uploaded droplet (5.1M)
2016-11-22T09:31:43.63-0700 [STG/0]      OUT Uploading complete
2016-11-22T09:31:43.70-0700 [STG/0]      OUT Destroying container
2016-11-22T09:31:44.10-0700 [CELL/0]     OUT Creating container
2016-11-22T09:31:45.57-0700 [STG/0]      OUT Successfully destroyed container
2016-11-22T09:31:47.32-0700 [CELL/0]     OUT Successfully created container
2016-11-22T09:31:51.59-0700 [APP/0]      OUT Running ..\tmp\lifecycle\WebAppServer.exe
2016-11-22T09:31:51.63-0700 [APP/0]      OUT PORT == 50706
2016-11-22T09:31:51.64-0700 [APP/0]      OUT 2016-11-22 16:31:51Z|INFO|Port:50706
2016-11-22T09:31:51.64-0700 [APP/0]      OUT 2016-11-22 16:31:51Z|INFO|Webroot:C:\containerizer\6897C4658B9FEC74DC\user\app
2016-11-22T09:31:51.72-0700 [APP/0]      OUT 2016-11-22 16:31:51Z|INFO|Starting web server instance...
2016-11-22T09:31:51.83-0700 [APP/0]      OUT Server Started.... press CTRL + C to stop
```

At this point the Fortune Teller Service is up and running and ready for the Fortune Teller UI to ask for fortunes.

---

### See the Official [Steeltoe Circuit Breaker Documentation](https://steeltoe.io/docs/steeltoe-circuitbreaker) for a more in-depth walkthrough of the samples and more detailed information