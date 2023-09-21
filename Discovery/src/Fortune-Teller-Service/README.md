# Fortune-Teller-Service - ASP.NET Core Microservice

ASP.NET Core sample app illustrating how to use [Spring Cloud Eureka Server](https://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#spring-cloud-eureka-server) for registering micro services. The Fortune-Teller-Service registers the fortuneService with the Eureka server upon startup.

Note: You can run this either locally or on CloudFoundry.

## Pre-requisites - Running Local

This sample assumes that there is a running Spring Cloud Eureka Server on your machine. To make this happen:

1. Install Java 8 JDK.
1. Install Maven 3.x.
1. Clone the Spring Cloud Samples Eureka repository. <https://github.com/spring-cloud-samples/eureka.git>
1. Go to the eureka server directory (`eureka`) and fire it up with `mvn spring-boot:run`
1. When running locally, this sample will default to looking for its eurka server on <http://localhost:8761/eureka>, so it should all connect.
1. Install .NET Core SDK

## Building & Running - Local

1. Clone this repo. (i.e. git clone <https://github.com/SteeltoeOSS/Samples>)
1. cd samples/Discovery/src/Fortune-Teller-Service
1. dotnet restore --configfile nuget.config
1. dotnet run

## What to expect - Local

After building and running the app, you should see something like the following:

```bash
$ cd samples/Discovery/src/Fortune-Teller-Service
$ dotnet run
info: Microsoft.Data.Entity.Storage.Internal.InMemoryStore[1]
      Saved 50 entities to in-memory store.
Hosting environment: Production
Now listening on: http://*:5000
Application started. Press Ctrl+C to shut down.
```

At this point the Fortune Teller Service is up and running and ready for the Fortune Teller UI to ask for fortunes.

## Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry
1. Optionally install Windows support
1. Installed Spring Cloud Services
1. Install .NET Core SDK

## Setup Service Registry on CloudFoundry

You must first create an instance of the Service Registry service in a org/space.

1. cf target -o myorg -s development
1. cf create-service p.service-registry standard myDiscoveryService
1. Wait for the service to become ready! (i.e. cf services)

## Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
1. cd samples/Discovery/src/Fortune-Teller-Service
1. Publish app to a directory selecting the framework and runtime you want to run on. (e.g. `dotnet publish -r linux-x64 --self-contained` or `dotnet publish -r win-x64 --self-contained`)
1. Push the app using the appropriate manifest. (e.g. `cf push -f manifest.yml -p bin/Debug/net6.0/linux-x64/publish` or `cf push -f manifest-windows.yml -p bin/Debug/net6.0/win-x64/publish`)

> Note: If you are using self-signed certificates it is possible that you might run into SSL certificate validation issues when pushing this app. The simplest way to fix this:

1. Disable certificate validation for the Spring Cloud Discovery Client.  You can do this by editing `appsettings.json` and add `Eureka:Client:ValidateCertificates=false`.

## What to expect - CloudFoundry

After building and running the app, you should see something like the following in the logs.

To see the logs as you startup and use the app: `cf logs fortuneservice`

On a Windows cell, you should see something like this during startup:

```bash
CELL/0] OUT Starting health monitoring of container
   2023-09-19T17:55:15.82+0200 [APP/PROC/WEB/0] OUT dbug: Steeltoe.Discovery.Eureka.Transport.EurekaHttpClient[0]
   2023-09-19T17:55:15.82+0200 [APP/PROC/WEB/0] OUT GetRequestContent generated JSON: {"instance":{"instanceId":"fortuneService-intelligent-quokka-zh.apps.clovis.cf-app.com:0fcfcd46-f4a2-4b0e-75aa-0f34","app":"FORTUNESERVICE","appGroupName":null,"ipAddr":"172.30.3.92","sid":"na","port":{"@enabled":"true","$":80},"securePort":{"@enabled":"false","$":443},"homePageUrl":"http://fortuneService-intelligent-quokka-zh.apps.clovis.cf-app.com:80/","statusPageUrl":"http://fortuneService-intelligent-quokka-zh.apps.clovis.cf-app.com:80/actuator/info","healthCheckUrl":"http://fortuneService-intelligent-quokka-zh.apps.clovis.cf-app.com:80/actuator/health","secureHealthCheckUrl":null,"vipAddress":"fortuneService","secureVipAddress":"fortuneService","countryId":1,"dataCenterInfo":{"@class":"com.netflix.appinfo.InstanceInfo$DefaultDataCenterInfo","name":"MyOwn"},"hostName":"fortuneService-intelligent-quokka-zh.apps.clovis.cf-app.com","status":"UP","overriddenstatus":"UNKNOWN","leaseInfo":{"renewalIntervalInSecs":30,"durationInSecs":90,"registrationTimestamp":"0","lastRenewalTimestamp":"0","renewalTimestamp":"0","evictionTimestamp":"0","serviceUpTimestamp":"0"},"isCoordinatingDiscoveryServer":"false","metadata":{"cfAppGuid":"06ccada2-c5b5-4306-97a6-900be018e700","cfInstanceIndex":"0","instanceId":"0fcfcd46-f4a2-4b0e-75aa-0f34","zone":"unknown"},"lastUpdatedTimestamp":"1695138914096","lastDirtyTimestamp":"1695138914096","actionType":"ADDED","asgName":null}}
   2023-09-19T17:55:15.83+0200 [APP/PROC/WEB/0] OUT info: System.Net.Http.HttpClient.Eureka.LogicalHandler[100]
   2023-09-19T17:55:15.83+0200 [APP/PROC/WEB/0] OUT Start processing HTTP request POST https://service-registry-4124f73a-5077-42fd-9aa2-a9ffb51fd879.apps.clovis.cf-app.com/eureka/apps/FORTUNESERVICE
   2023-09-19T17:55:15.83+0200 [APP/PROC/WEB/0] OUT info: System.Net.Http.HttpClient.Eureka.ClientHandler[100]
   2023-09-19T17:55:15.83+0200 [APP/PROC/WEB/0] OUT Sending HTTP request POST https://service-registry-4124f73a-5077-42fd-9aa2-a9ffb51fd879.apps.clovis.cf-app.com/eureka/apps/FORTUNESERVICE
   2023-09-19T17:55:15.93+0200 [APP/PROC/WEB/0] OUT info: System.Net.Http.HttpClient.Eureka.ClientHandler[101]
   2023-09-19T17:55:15.93+0200 [APP/PROC/WEB/0] OUT Received HTTP response headers after 74.6557ms - 204
   2023-09-19T17:55:15.93+0200 [APP/PROC/WEB/0] OUT info: System.Net.Http.HttpClient.Eureka.LogicalHandler[101]
   2023-09-19T17:55:15.93+0200 [APP/PROC/WEB/0] OUT End processing HTTP request after 94.694ms - 204
   2023-09-19T17:55:15.93+0200 [APP/PROC/WEB/0] OUT dbug: Steeltoe.Discovery.Eureka.Transport.EurekaHttpClient[0]
   2023-09-19T17:55:15.93+0200 [APP/PROC/WEB/0] OUT RegisterAsync https://service-registry-4124f73a-5077-42fd-9aa2-a9ffb51fd879.apps.clovis.cf-app.com/eureka/apps/FORTUNESERVICE, status: NoContent, retry: 0
   2023-09-19T17:55:15.93+0200 [APP/PROC/WEB/0] OUT info: Startup.Steeltoe.Discovery.Eureka.EurekaDiscoveryClient[0]
   2023-09-19T17:55:15.93+0200 [APP/PROC/WEB/0] OUT Starting HeartBeat
   2023-09-19T17:55:17.12+0200 [APP/PROC/WEB/0] OUT info: Microsoft.EntityFrameworkCore.Infrastructure[10403]
   2023-09-19T17:55:17.12+0200 [APP/PROC/WEB/0] OUT Entity Framework Core 6.0.22 initialized 'FortuneContext' using provider 'Microsoft.EntityFrameworkCore.InMemory:6.0.22' with options: StoreName=Fortunes
   2023-09-19T17:55:17.15+0200 [APP/PROC/WEB/0] OUT info: Microsoft.EntityFrameworkCore.Update[30100]
   2023-09-19T17:55:17.15+0200 [APP/PROC/WEB/0] OUT Saved 0 entities to in-memory store.
   2023-09-19T17:55:17.33+0200 [APP/PROC/WEB/0] OUT info: Microsoft.EntityFrameworkCore.Update[30100]
   2023-09-19T17:55:17.33+0200 [APP/PROC/WEB/0] OUT Saved 50 entities to in-memory store.
   2023-09-19T17:55:17.52+0200 [APP/PROC/WEB/0] OUT dbug: Steeltoe.Discovery.Eureka.Transport.EurekaHttpClient[0]
   2023-09-19T17:55:17.52+0200 [APP/PROC/WEB/0] OUT GetRequestContent generated JSON: {"instance":{"instanceId":"fortuneService-intelligent-quokka-zh.apps.clovis.cf-app.com:0fcfcd46-f4a2-4b0e-75aa-0f34","app":"FORTUNESERVICE","appGroupName":null,"ipAddr":"172.30.3.92","sid":"na","port":{"@enabled":"true","$":80},"securePort":{"@enabled":"false","$":443},"homePageUrl":"http://fortuneService-intelligent-quokka-zh.apps.clovis.cf-app.com:80/","statusPageUrl":"http://fortuneService-intelligent-quokka-zh.apps.clovis.cf-app.com:80/actuator/info","healthCheckUrl":"http://fortuneService-intelligent-quokka-zh.apps.clovis.cf-app.com:80/actuator/health","secureHealthCheckUrl":null,"vipAddress":"fortuneService","secureVipAddress":"fortuneService","countryId":1,"dataCenterInfo":{"@class":"com.netflix.appinfo.InstanceInfo$DefaultDataCenterInfo","name":"MyOwn"},"hostName":"fortuneService-intelligent-quokka-zh.apps.clovis.cf-app.com","status":"UP","overriddenstatus":"UNKNOWN","leaseInfo":{"renewalIntervalInSecs":30,"durationInSecs":90,"registrationTimestamp":"0","lastRenewalTimestamp":"0","renewalTimestamp":"0","evictionTimestamp":"0","serviceUpTimestamp":"0"},"isCoordinatingDiscoveryServer":"false","metadata":{"cfAppGuid":"06ccada2-c5b5-4306-97a6-900be018e700","cfInstanceIndex":"0","instanceId":"0fcfcd46-f4a2-4b0e-75aa-0f34","zone":"unknown"},"lastUpdatedTimestamp":"1695138914096","lastDirtyTimestamp":"1695138914096","actionType":"ADDED","asgName":null}}
   2023-09-19T17:55:17.52+0200 [APP/PROC/WEB/0] OUT info: System.Net.Http.HttpClient.Eureka.LogicalHandler[100]
   2023-09-19T17:55:17.52+0200 [APP/PROC/WEB/0] OUT Start processing HTTP request POST https://service-registry-4124f73a-5077-42fd-9aa2-a9ffb51fd879.apps.clovis.cf-app.com/eureka/apps/FORTUNESERVICE
   2023-09-19T17:55:17.52+0200 [APP/PROC/WEB/0] OUT info: System.Net.Http.HttpClient.Eureka.ClientHandler[100]
   2023-09-19T17:55:17.52+0200 [APP/PROC/WEB/0] OUT Sending HTTP request POST https://service-registry-4124f73a-5077-42fd-9aa2-a9ffb51fd879.apps.clovis.cf-app.com/eureka/apps/FORTUNESERVICE
   2023-09-19T17:55:17.54+0200 [APP/PROC/WEB/0] OUT info: System.Net.Http.HttpClient.Eureka.ClientHandler[101]
   2023-09-19T17:55:17.54+0200 [APP/PROC/WEB/0] OUT Received HTTP response headers after 26.902ms - 204
   2023-09-19T17:55:17.54+0200 [APP/PROC/WEB/0] OUT info: System.Net.Http.HttpClient.Eureka.LogicalHandler[101]
   2023-09-19T17:55:17.54+0200 [APP/PROC/WEB/0] OUT End processing HTTP request after 27.1231ms - 204
   2023-09-19T17:55:17.54+0200 [APP/PROC/WEB/0] OUT dbug: Steeltoe.Discovery.Eureka.Transport.EurekaHttpClient[0]
   2023-09-19T17:55:17.54+0200 [APP/PROC/WEB/0] OUT RegisterAsync https://service-registry-4124f73a-5077-42fd-9aa2-a9ffb51fd879.apps.clovis.cf-app.com/eureka/apps/FORTUNESERVICE, status: NoContent, retry: 0
   2023-09-19T17:55:17.54+0200 [APP/PROC/WEB/0] OUT info: Startup.Steeltoe.Discovery.Eureka.EurekaDiscoveryClient[0]
   2023-09-19T17:55:17.54+0200 [APP/PROC/WEB/0] OUT Starting HeartBeat
   2023-09-19T17:55:17.58+0200 [APP/PROC/WEB/0] OUT info: Microsoft.Hosting.Lifetime[14]
   2023-09-19T17:55:17.58+0200 [APP/PROC/WEB/0] OUT Now listening on: http://0.0.0.0:8080
   2023-09-19T17:55:17.58+0200 [APP/PROC/WEB/0] OUT info: Steeltoe.Management.Diagnostics.DiagnosticObserver[0]
   2023-09-19T17:55:17.58+0200 [APP/PROC/WEB/0] OUT DiagnosticObserver TraceDiagnosticObserver Subscribed to Microsoft.AspNetCore
   2023-09-19T17:55:17.58+0200 [APP/PROC/WEB/0] OUT info: Steeltoe.Management.Diagnostics.DiagnosticObserver[0]
   2023-09-19T17:55:17.58+0200 [APP/PROC/WEB/0] OUT DiagnosticObserver AspNetCoreHostingObserver Subscribed to Microsoft.AspNetCore
   2023-09-19T17:55:17.59+0200 [APP/PROC/WEB/0] OUT info: Microsoft.Hosting.Lifetime[0]
   2023-09-19T17:55:17.59+0200 [APP/PROC/WEB/0] OUT Application started. Press Ctrl+C to shut down.
```

At this point the Fortune Teller Service is up and running and ready for the Fortune Teller UI to ask for fortunes.

## Using Container-to-Container on Cloud Foundry

If you wish to use Container-to-Container (C2C) communications between the UI and the Fortune Service, look at the comments in the files listed below.  You will need to make modifications to those files.  Also, you are encouraged to read the Cloud Foundry [C2C documentation](https://docs.pivotal.io/pivotalcf/1-10/devguide/deploy-apps/cf-networking.html) FIRST, before trying to use it with the Fortune Teller app.

1. `appsettings.json` - Eureka registration option settings

## Enabling SSL usage on Cloud Foundry

If you wish to use SSL communications between the Fortune Teller UI and the Fortune Teller Service, have a look at the comments in the files listed below.  You will need to make modifications to one or more of those files. Also, you are encouraged to read the [Cloud Foundry documentation](https://docs.pivotal.io/pivotalcf/1-10/adminguide/securing-traffic.html) on how SSL is configured, used and implemented before trying to use it with the Fortune Teller app.

1. `appsettings.json` - Eureka registration option settings
2. `Program.cs` - Changes needed to enable SSL usage with Kestrel (Note: These changes are only required if using Container-to-Container (C2C) networking, together with SSL, between the UI and the fortune service).
3. `manifest.yml` - Startup command line change (Note: This change is only required if using Container-to-Container (C2C) networking, together with SSL, between the UI and the fortune service).

---

### See the Official [Steeltoe Service Discovery Documentation](https://steeltoe.io/docs/steeltoe-service-discovery) for a more in-depth walkthrough of the samples and more detailed information
