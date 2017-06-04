# Fortune-Teller-Service - ASP.NET Core Microservice
ASP.NET Core sample app illustrating how to use [Spring Cloud Eureka Server](http://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#spring-cloud-eureka-server) for registering micro services. The Fortune-Teller-Service registers the fortuneService with the Eureka server upon startup.

Note: You can run this either locally or on CloudFoundry.

# Pre-requisites - Running Local

This sample assumes that there is a running Spring Cloud Eureka Server on your machine. To make this happen:

1. Install Java 8 JDK.
2. Install Maven 3.x.
3. Clone the Spring Cloud Samples Eureka repository. (https://github.com/spring-cloud-samples/eureka.git)
4. Go to the eureka server directory (`eureka`) and fire it up with `mvn spring-boot:run`
5. When running locally, this sample will default to looking for its eurka server on http://localhost:8761/eureka, so it should all connect.
6. Install .NET Core SDK 

# Building & Running - Local

1. Clone this repo. (i.e. git clone https://github.com/SteeltoeOSS/Samples)
2. cd samples/Discovery/src/AspDotNetCore/Fortune-Teller-Service
3. dotnet restore --configfile nuget.config
4. dotnet run -f netcoreapp1.1 --server.urls http://*:5000

# What to expect - Local
After building and running the app, you should see something like the following:
```
$ cd samples/Discovery/src/AspDotNetCore/Fortune-Teller-Service
$ dotnet run -f netcoreapp1.1 --server.urls http://*:5000
info: Microsoft.Data.Entity.Storage.Internal.InMemoryStore[1]
      Saved 50 entities to in-memory store.
Hosting environment: Production
Now listening on: http://*:5000
Application started. Press Ctrl+C to shut down.
```
At this point the Fortune Teller Service is up and running and ready for the [Fortune Teller UI]() to ask for fortunes.

# Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry 1.7+
2. Optionally install DiegoWindows support (Greenhouse)
3. Installed Spring Cloud Services 1.0.9+
4. Install .NET Core SDK


# Setup Service Registry on CloudFoundry
You must first create an instance of the Service Registry service in a org/space.

1. cf target -o myorg -s development
2. cf create-service p-service-registry standard myDiscoveryService 
3. Wait for the service to become ready! (i.e. cf services)

# Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
2. cd samples/Discovery/src/AspDotNetCore/Fortune-Teller-Service
3. dotnet restore --configfile nuget.config
4. Publish app to a directory selecting the framework and runtime you want to run on. 
(e.g. `dotnet publish --output $PWD/publish --configuration Release --framework netcoreapp1.1 --runtime ubuntu.14.04-x64`)
5. Push the app using the appropriate manifest.
 (e.g. `cf push -f manifest.yml -p $PWD/publish` or `cf push -f manifest-windows.yml -p $PWD/publish`)

Note: If you are using self-signed certificates it is possible that you might run into SSL certificate validation issues when pushing this app. The simplest way to fix this:

1. Disable certificate validation for the Spring Cloud Discovery Client.  You can do this by editing `appsettings.json` and add `eureka:client:validate_certificates=false`.

# What to expect - CloudFoundry
After building and running the app, you should see something like the following in the logs. 

To see the logs as you startup and use the app: `cf logs fortuneservice`

On a Windows cell, you should see something like this during startup:
```
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
At this point the Fortune Teller Service is up and running and ready for the [Fortune Teller UI]() to ask for fortunes.
