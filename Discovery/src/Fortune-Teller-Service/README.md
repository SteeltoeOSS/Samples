# Fortune-Teller-Service - ASP.NET 5 Microservice
ASP.NET 5 sample app illustrating how to use [Spring Cloud Eureka Server](http://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#spring-cloud-eureka-server) for registering micro services. The Fortune-Teller-Service registers the fortuneService with the Eureka server upon startup.

# Pre-requisites - Local

This sample assumes that there is a running Spring Cloud Eureka Server on your machine. To make this happen

1. Install Java 8 JDK.
2. Install Maven 3.x.
3. Clone the Spring Cloud Samples Eureka repository. (https://github.com/spring-cloud-samples/eureka.git)
4. Go to the eureka server directory (`eureka`) and fire it up with `mvn spring-boot:run`
5. This sample will default to looking for its eurka server on http://localhost:8761/eureka, so it should all connect.


# Building & Running - Local

1. Clone this repo. (i.e. git clone https://github.com/SteelToeOSS/Samples)
2. cd samples/Discovery/src/Fortune-Teller-Service
3. Install DNX 1.0.0-rc1-final/update1. Install either the coreclr and/or clr runtimes. 
4. Add a DNX runtime to your path. (e.g. dnvm use 1.0.0-rc1-update1 -r clr)
5. dnu restore
6. dnx kestrel

# What to expect - Local
After building and running the app, you should see something like the following:
```
$ cd src/Fortune-Teller-Service
$ dnx web
info: Microsoft.Data.Entity.Storage.Internal.InMemoryStore[1]
      Saved 50 entities to in-memory store.
Hosting environment: Production
Now listening on: http://*:5000
Application started. Press Ctrl+C to shut down.
```
At this point the Fortune Teller Service is up and running and ready for the [Fortune Teller UI]() to ask for fortunes.

# Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry 1.7
2. Installed Spring Cloud Services 1.0.9
3. Web tools installed and on PATH, (e.g. npm, gulp, etc).  
Note: If your on Windows and you have VS2015 Update 1, you can add these to your path: `C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\WebTemplates\DNX\CSharp\1033\StarterWeb\node_modules\.bin` and `C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Extensions\Microsoft\Web Tools\External` and you should get what you need.

# Setup Service Registry on CloudFoundry
You must first create an instance of the Service Registry service in a org/space.

1. cf target -o myorg -s development
2. cf create-service p-service-registry standard myDiscoveryService 

# Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
2. cd src/Fortune-Teller-Service
3. dnu restore
4. cd ..
5. Publish app to a directory selecting the runtime you want to run on. 
(e.g. `dnu publish --out ./publish --configuration Release  --runtime /usr/local/lib/dnx/runtimes/dnx-coreclr-linux-x64.1.0.0-rc1-update1/ Fortune-Teller-Service/`)
6. Push the app using the appropriate manifest.
 (e.g. `cf push -f Fortune-Teller-Service/manifest.yml -p ./publish` or `cf push -f Fortune-Teller-Service/manifest-windows.yml -p ./publish`)

Windows Note: If you are pushing to a windows stack, and you are using self-signed certificates you are likely to run into SSL certificate validation issues when pushing this app. You have two choices to fix this:

1. If you have created your own ROOT CA and from it created a certificate that you have installed in HAProxy/Ext LB, then you can install the ROOT CA on the windows cells and you would be good to go.
2. Disable certificate validation for the Spring Cloud Discovery Client.  You can do this by editing `appsettings.json` and add `spring:cloud:client:validate_certificates=false`. This only works on Windows, it will not work on CoreCLR/Linux.

Note: We have experienced this [problem](https://github.com/aspnet/KestrelHttpServer/issues/341) with Kestrel running behind a proxy (e.g. HAProxy/Nginx, etc.). As a result, currently this app is configured to run using the `Microsoft.AspNet.Server.WebListener`; which only runs on Windows. If you'd like to try it on Linux, you can change `project.json` to use Kestrel and see what happens. We will change this when moving to RC2 bits.

# What to expect - CloudFoundry
After building and running the app, you should see something like the following in the logs. 

To see the logs as you startup and use the app: `cf logs fortuneservice`

On a Windows cell, you should see something like this during startup:
```
2016-05-14T06:22:39.54-0600 [APP/0]      OUT dbug: SteelToe.Discovery.Eureka.Transport.EurekaHttpClient[0]
2016-05-14T06:22:39.54-0600 [APP/0]      OUT       GetRequestContent generated JSON: ......
2016-05-14T06:22:39.57-0600 [APP/0]      OUT dbug: SteelToe.Discovery.Eureka.Transport.EurekaHttpClient[0]
2016-05-14T06:22:39.58-0600 [APP/0]      OUT       RegisterAsync .....
2016-05-14T06:22:39.58-0600 [APP/0]      OUT dbug: SteelToe.Discovery.Eureka.DiscoveryClient[0]
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
2016-05-14T06:23:09.76-0600 [APP/0]      OUT dbug: SteelToe.Discovery.Eureka.Transport.EurekaHttpClient[0]
2016-05-14T06:23:09.76-0600 [APP/0]      OUT       SendHeartbeatAsync ......., status: OK, instanceInfo: null
2016-05-14T06:23:09.76-0600 [APP/0]      OUT dbug: SteelToe.Discovery.Eureka.DiscoveryClient[0]
2016-05-14T06:23:09.76-0600 [APP/0]      OUT       Renew FORTUNESERVICE/fortuneService.apps.testcloud.com:2f7a9e48-bb3e-402a-6b44-68e9386b3b15 returned: OK
```
At this point the Fortune Teller Service is up and running and ready for the [Fortune Teller UI]() to ask for fortunes.
