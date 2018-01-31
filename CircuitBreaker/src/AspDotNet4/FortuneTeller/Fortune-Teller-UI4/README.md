# Fortune-Teller-UI - ASP.NET 4 MVC Application

ASP.NET 4 MVC sample app illustrating how to use [Spring Cloud Eureka Server](http://projects.spring.io/spring-cloud) for discovering micro services and [Spring Cloud Hystrix](http://cloud.spring.io/spring-cloud) for building resilient micro services applications. The Fortune-Teller-Service registers the fortuneService with the Eureka server upon startup and the Fortune-Teller-UI uses a Hystrix command with fallback ability when communicating with the Fortune service.

This sample also illustrates how to use the [Hystrix Dashboard](http://cloud.spring.io/spring-cloud) to gather status and metrics of the Hystrix command used in communications.

 The Fortune-Teller-UI attempts to locate the fortuneService in the Eureka server and uses it to get your fortune.

> Note: This application is built using the Autofac IOC container.

## Pre-requisites - Running on CloudFoundry

1. Installed Pivotal CloudFoundry with Windows support
1. Installed Spring Cloud Services

## Setup Service Registry on CloudFoundry

You must first create an instance of the Service Registry service in a org/space.

1. cf target -o myorg -s development
1. cf create-service p-service-registry standard myDiscoveryService

## Setup Circuit Breaker Dashboard service on CloudFoundry

You must first create an instance of the Circuit Breaker service in a org/space.

1. cf target -o myorg -s development
1. cf create-service p-circuit-breaker-dashboard standard myHystrixService
1. Wait for the service to become ready! (use `cf services` to check the status)

## Publish App & Push to CloudFoundry

1. Open Samples\CircuitBreaker\src\AspDotNet4\FortuneTeller.sln in Visual Studio.
1. Select Fortune-Teller-UI4 project in Solution Explorer.
1. Right-click and select Publish
1. Publish the App to a folder. (e.g. c:\publish)
1. cd publish_folder (e.g. cd c:\publish)
1. cf push

> Windows Note: If you are using self-signed certificates you are likely to run into SSL certificate validation issues when pushing this app. You have two choices to fix this:

1. If you have created your own ROOT CA and from it created a certificate that you have installed in HAProxy/Ext LB, then you can install the ROOT CA on the windows cells and you would be good to go.
1. Disable certificate validation for the Spring Cloud Discovery Client.  You can do this by editing `appsettings.json` and add `eureka:client:validate_certificates=false`.

## What to expect - CloudFoundry

After building and running the app, you should see something like the following in the logs.

To see the logs as you startup and use the app: `cf logs fortuneui`

You should see something like this during startup:

```bash
2016-11-22T09:47:41.84-0700 [STG/0]      OUT Successfully created container
2016-11-22T09:47:41.85-0700 [STG/0]      OUT Downloading app package...
2016-11-22T09:47:45.95-0700 [STG/0]      OUT Downloaded app package (6.5M)
2016-11-22T09:47:45.95-0700 [STG/0]      OUT Staging...
2016-11-22T09:47:50.80-0700 [STG/0]      OUT Exit status 0
2016-11-22T09:47:50.80-0700 [STG/0]      OUT Staging complete
2016-11-22T09:47:50.80-0700 [STG/0]      OUT Uploading droplet, build artifacts cache...
2016-11-22T09:47:50.80-0700 [STG/0]      OUT Uploading build artifacts cache...
2016-11-22T09:47:50.80-0700 [STG/0]      OUT Uploading droplet...
2016-11-22T09:47:50.87-0700 [STG/0]      OUT Uploaded build artifacts cache (88B)
2016-11-22T09:47:56.39-0700 [STG/0]      OUT Uploaded droplet (6.4M)
2016-11-22T09:47:56.39-0700 [STG/0]      OUT Uploading complete
2016-11-22T09:47:56.46-0700 [STG/0]      OUT Destroying container
2016-11-22T09:47:56.78-0700 [CELL/0]     OUT Creating container
2016-11-22T09:47:58.33-0700 [STG/0]      OUT Successfully destroyed container
2016-11-22T09:47:59.70-0700 [CELL/0]     OUT Successfully created container
2016-11-22T09:48:04.77-0700 [APP/0]      OUT Running ..\tmp\lifecycle\WebAppServer.exe
2016-11-22T09:48:04.84-0700 [APP/0]      OUT PORT == 51163
2016-11-22T09:48:04.84-0700 [APP/0]      OUT 2016-11-22 16:48:04Z|INFO|Webroot:C:\containerizer\2847B8BB744A0F4D78\user\app
2016-11-22T09:48:04.84-0700 [APP/0]      OUT 2016-11-22 16:48:04Z|INFO|Port:51163
2016-11-22T09:48:04.93-0700 [APP/0]      OUT 2016-11-22 16:48:04Z|INFO|Starting web server instance...
2016-11-22T09:48:05.04-0700 [APP/0]      OUT Server Started.... press CTRL + C to stop
```

At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit <http://fortuneui.x.y.z/> to see it!

## Using the Hystrix Dashboard - Cloud Foundry

Once you have the two applications communicating, you can make use of the Hystrix dashboard by following the instructions below.

1. Open a browser or browser window and connect to the Pivotal Apps Manager.  You will have to use a link that is specific to your Cloud Foundry setup. (e.g. <https://apps.system.testcloud.com>)
2. Follow [these instructions](http://docs.pivotal.io/spring-cloud-services/1-3/common/circuit-breaker/using-the-dashboard.html) to open the Hystrix dashboard service.
3. Go back to the Fortune-Teller-UI application and obtain several fortunes.  Observe the values changing in the Hystrix dashboard.  Click the refresh button on the UI app quickly to see the dashboard update.

---

### See the Official [Steeltoe Circuit Breaker Documentation](https://steeltoe.io/docs/steeltoe-circuitbreaker) for a more in-depth walkthrough of the samples and more detailed information