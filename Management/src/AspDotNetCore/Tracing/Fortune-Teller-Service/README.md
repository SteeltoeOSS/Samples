# Fortune-Teller-Service - ASP.NET Core Micro-service

ASP.NET Core sample app illustrating how to use the Tracing features found in the Steeltoe Management framework. This sample requires running two additional services in order for it to run properly.

First, a [Spring Cloud Eureka Server](http://cloud.spring.io/spring-cloud-static/Edgware.SR3/multi/multi_spring-cloud-eureka-server.html) for registering micro services. The Fortune-Teller-Service registers the fortuneService with the Eureka server upon startup.  The Fortune-Teller-UI will discover the service using the Steeltoe Eureka client.

Second, a [Zipkin Server](https://zipkin.io/pages/quickstart) for capturing and viewing trace information captured by both Fortune-Teller components.

Note: You can run all of this either locally or on CloudFoundry.

## Pre-requisites - Running Locally

### Running Eureka Server Locally

This sample assumes that there is a running Spring Cloud Eureka Server on your machine. To make this happen:

1. Install Java 8 JDK.
1. Install Maven 3.x.
1. Clone the Spring Cloud Samples Eureka repository. <https://github.com/spring-cloud-samples/eureka.git>
1. Go to the eureka server directory (`eureka`) and fire it up with `mvn spring-boot:run`
1. When running locally, this sample will default to looking for its eureka server on <http://localhost:8761/eureka>, so it should all connect.

### Running Eureka Server Locally - Docker

If you have a running docker environment installed on your system, then you should be able to:

1. docker run -d -p:8761:8761 steeltoeoss/eurekaserver

### Running Zipkin Server Locally

1. Follow the instructions in the [Zipkin Quickstart](https://zipkin.io/pages/quickstart) guide. You will need Java installed to do this.

### Running Zipkin Server Locally - Docker

If you have a running docker environment installed on your system, then you should be able to:

1. docker run -d -p 9411:9411 openzipkin/zipkin

## Building & Running - Locally

1. Clone this repository. (i.e. git clone <https://github.com/SteeltoeOSS/Samples>)
1. cd samples/Management/src/AspDotNetCore/Tracing/Fortune-Teller-Service
1. dotnet run -f netcoreapp2.1

## What to expect - Locally

After building and running the app, you should see something like the following:

```bash
$ cd samples/Management/src/AspDotNetCore/Tracing/Fortune-Teller-Service
$ dotnet run -f netcoreapp2.1
info: Microsoft.Data.Entity.Storage.Internal.InMemoryStore[1]
      Saved 50 entities to in-memory store.
Hosting environment: Production
Now listening on: http://*:5000
Application started. Press Ctrl+C to shut down.
```

At this point the Fortune Teller Service is up and running and ready for the Fortune Teller UI to ask for fortunes.  Go to the Fortune-Teller-UI directory for details on how to start it up.

## Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry with Docker enabled on Diego
1. Optionally install Windows support
1. Installed Spring Cloud Services
1. Install .NET Core SDK

## Setup Service Registry on CloudFoundry

You must first create an instance of the Service Registry service in a org/space.

1. cf target -o myorg -s development
1. cf create-service p-service-registry standard myDiscoveryService
1. Wait for the service to become ready! (i.e. cf services)

## Run Zipkin Server on Cloud Foundry

1. Download latest zipkin server jar file from [here](https://dl.bintray.com/openzipkin/maven/io/zipkin/java/zipkin-server) (e.g. zipkin-server-2.8.4-exec.jar).
1. Start the zipkin server on Cloud Foundry. (e.g. cf push zipkin-server -p ./zipkin-server-2.8.4-exec.jar)
1. Verify server is up and running.   (e.g. http://zipkin-server.cfapps.io/)

## Configure Zipkin Server Endpoint in Fortune-Teller-Service

1. Open `appsettings.json` and modify the `management:tracing:exporter:zipkin:endpoint` configuration setting to match the endpoint of the Zipkin server deployed to Cloud Foundry above.  (e.g. `http://zipkin-server.cfapps.io/api/v2/spans`)

## Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
1. cd samples/Discovery/src/AspDotNetCore/Fortune-Teller-Service
1. Publish app to a directory selecting the framework and runtime you want to run on. (e.g. `dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64`)
1. Push the app using the appropriate manifest. (e.g. `cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish` or `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish`)

## What to expect - CloudFoundry

After building and running the app, you should see something like the following in the logs.

To see the logs as you startup and use the app: `cf logs fortuneservice`

On a Windows cell, you should see something like this during startup:

```bash
2016-05-14T06:22:39.54-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.Transport.EurekaHttpClient[0]
2016-05-14T06:22:39.54-0600 [APP/0]      OUT       GetRequestContent generated JSON: ......
2016-05-14T06:22:39.57-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.Transport.EurekaHttpClient[0]
2016-05-14T06:22:39.58-0600 [APP/0]      OUT       RegisterAsync .....
2016-05-14T06:22:39.58-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.DiscoveryClient[0]
2016-05-14T06:22:39.58-0600 [APP/0]      OUT       Register FORTUNESERVICE/fortuneService.apps.testcloud.com:2f7a9e48-bb3e-402a-6b44-68e9386b3b15 returned: NoContent
2016-05-14T06:22:41.07-0600 [APP/0]      OUT info: Microsoft.Data.Entity.Storage.Internal.InMemoryStore[1]
2016-05-14T06:22:41.07-0600 [APP/0]      OUT       Saved 50 entities to in-memory store.
2016-05-14T06:22:41.17-0600 [APP/0]      OUT       Hosting starting
2016-05-14T06:22:41.17-0600 [APP/0]      OUT verb: Microsoft.AspNet.Hosting.Internal.HostingEngine[4]
2016-05-14T06:22:41.19-0600 [APP/0]      OUT       Start
2016-05-14T06:22:41.19-0600 [APP/0]      OUT info: Microsoft.Net.Http.Server.WebListener[0]
2016-05-14T06:22:41.23-0600 [APP/0]      OUT       Listening on prefix: http://*:57991/
2016-05-14T06:22:41.23-0600 [APP/0]      OUT info: Microsoft.Net.Http.Server.WebListener[0]
2016-05-14T06:22:41.31-0600 [APP/0]      OUT verb: Microsoft.AspNet.Hosting.Internal.HostingEngine[5]
2016-05-14T06:22:41.31-0600 [APP/0]      OUT       Hosting started
2016-05-14T06:22:41.31-0600 [APP/0]      OUT Hosting environment: development
2016-05-14T06:22:41.32-0600 [APP/0]      OUT Now listening on: http://*:57991
2016-05-14T06:22:41.32-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
2016-05-14T06:23:09.76-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.Transport.EurekaHttpClient[0]
2016-05-14T06:23:09.76-0600 [APP/0]      OUT       SendHeartbeatAsync ......., status: OK, instanceInfo: null
2016-05-14T06:23:09.76-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.DiscoveryClient[0]
2016-05-14T06:23:09.76-0600 [APP/0]      OUT       Renew FORTUNESERVICE/fortuneService.apps.testcloud.com:2f7a9e48-bb3e-402a-6b44-68e9386b3b15 returned: OK
```

At this point the Fortune Teller Service is up and running and ready for the Fortune Teller UI to ask for fortunes. Go to the Fortune-Teller-UI directory for details on how to start it up.