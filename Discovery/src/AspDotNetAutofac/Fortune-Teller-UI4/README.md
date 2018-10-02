# Fortune-Teller-UI - ASP.NET 4 MVC Application

ASP.NET 4 MVC sample app illustrating how to use [Spring Cloud Eureka Server](http://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#spring-cloud-eureka-server) for discovering micro services. The Fortune-Teller-UI attempts to locate the fortuneService in the Eureka server and uses it to get your fortune.

> Note: This application is built using the Autofac IOC container.

## Pre-requisites - Running Local

This sample assumes that there is a running Spring Cloud Eureka Server on your machine. To make this happen:

1. Install Java 8 JDK.
1. Install Maven 3.x.
1. Clone the Spring Cloud Samples Eureka repository. (<https://github.com/spring-cloud-samples/eureka.git>)
1. Go to the eureka server directory (`eureka`) and fire it up with `mvn spring-boot:run`
1. This sample will default to looking for its eurka server on <http://localhost:8761/eureka>, so it should all connect.

## Building & Running - Local

1. Clone this repo. (e.g. `git clone https://github.com/SteeltoeOSS/Samples`)
1. Startup Visual Studio 2015
1. Open samples/Discovery/Discovery.sln
1. Select AspDotNet4/Fortune-Teller-UI4 and build it
1. Select AspDotNet4/Fortune-Teller-UI4 as the Startup project.
1. Ctrl+F5 or F5

At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit <http://localhost:5555/> to see it!  Of course you have to have the FortuneService up and running first!

## Pre-requisites - Running on CloudFoundry

1. Installed Pivotal CloudFoundry 1.7+ with Greenhouse (i.e. Windows cell)
1. Installed Spring Cloud Services 1.0.9+
1. Web tools installed and on PATH, (e.g. npm, gulp, etc).

> Note: If you're on Windows and you have VS2015 Update 3, you can add these to your path: C:\Program Files (x86)\Microsoft Visual Studio 14.0\Web\External.

## Setup Service Registry on CloudFoundry

You must first create an instance of the Service Registry service in a org/space.

1. cf target -o myorg -s development
1. cf create-service p-service-registry standard myDiscoveryService

## Publish App & Push to CloudFoundry

1. Open Samples\Discovery\Discovery.sln in Visual Studio 2015.
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

At this point the Fortune Teller UI is up and running, ready for displaying your fortune. Hit <http://fortuneui.x.y.z/> to see it!

---

### See the Official [Steeltoe Service Discovery Documentation](https://steeltoe.io/docs/steeltoe-service-disovery) for a more in-depth walkthrough of the samples and more detailed information