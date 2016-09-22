# Fortune-Teller-UI - ASP.NET 5 MVC Application
ASP.NET 5 sample app illustrating how to use [Spring Cloud Eureka Server](http://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#spring-cloud-eureka-server) for discovering micro services. The Fortune-Teller-UI attempts to locate the fortuneService in the Eureka server and uses it to get your fortune.

# Pre-requisites - Local

This sample assumes that there is a running Spring Cloud Eureka Server on your machine. To make this happen:

1. Install Java 8 JDK.
2. Install Maven 3.x.
3. Clone the Spring Cloud Samples Eureka repository. (https://github.com/spring-cloud-samples/eureka.git)
4. Go to the eureka server directory (`eureka`) and fire it up with `mvn spring-boot:run`
5. This sample will default to looking for its eurka server on http://localhost:8761/eureka, so it should all connect.
6. Install .NET Core SDK

# Building & Running - Local

1. Clone this repo. (i.e. git clone https://github.com/SteeltoeOSS/Samples)
2. cd samples/Discovery/src/Fortune-Teller-UI
3. dotnet restore --configfile nuget.config
4. dotnet run --server.urls http://*:5555


# What to expect - Local
After building and running the app, you should see something like the following:
```
$ cd samples/Discovery/src/Fortune-Teller-UI
$ dotnet run --server.urls http://*:5555
Hosting environment: Production
Now listening on: http://*:5555
Application started. Press Ctrl+C to shut down.
```
At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit http://localhost:5555/ to see it!

# Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry 1.7
2. Installed Spring Cloud Services 1.0.9
3. Install .NET Core SDK
4. Web tools installed and on PATH, (e.g. npm, gulp, etc).  
Note: If your on Windows and you have VS2015 Update 1, you can add these to your path: `C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\WebTemplates\DNX\CSharp\1033\StarterWeb\node_modules\.bin` and `C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Extensions\Microsoft\Web Tools\External` and you should get what you need.

# Setup Service Registry on CloudFoundry
You must first create an instance of the Service Registry service in a org/space.

1. cf target -o myorg -s development
2. cf create-service p-service-registry standard myDiscoveryService 

# Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
2. cd samples/Discovery/src/Fortune-Teller-UI
3. dotnet restore --configfile nuget.config
4. Publish app to a directory selecting the framework and runtime you want to run on. 
(e.g. `dotnet publish --output $PWD/publish --configuration Release --framework netcoreapp1.0 --runtime ubuntu.14.04-x64`)
5. Push the app using the appropriate manifest.
 (e.g. `cf push -f manifest.yml -p $PWD/publish` or `cf push -f manifest-windows.yml -p $PWD/publish`)

Windows Note: If you are pushing to a windows stack, and you are using self-signed certificates you are likely to run into SSL certificate validation issues when pushing this app. You have two choices to fix this:

1. If you have created your own ROOT CA and from it created a certificate that you have installed in HAProxy/Ext LB, then you can install the ROOT CA on the windows cells and you would be good to go.
2. Disable certificate validation for the Spring Cloud Discovery Client.  You can do this by editing `appsettings.json` and add `spring:cloud:client:validate_certificates=false`. This only works on Windows, it will not work on CoreCLR/Linux.

Note: We have experienced this [problem](https://github.com/dotnet/cli/issues/3283) when using the RC2 SDK and when publishing to a relative directory... workaround is to use full path.

# What to expect - CloudFoundry
After building and running the app, you should see something like the following in the logs. 

To see the logs as you startup and use the app: `cf logs fortuneui`

On a Windows cell, you should see something like this during startup:
```
2016-05-14T06:38:21.67-0600 [CELL/0]     OUT Successfully created container
2016-05-14T06:38:27.78-0600 [APP/0]      OUT Running cmd /c SET "DNX_PACKAGES=%CD%\approot\packages" & approot\web.cmd --server.urls http://*:%PORT%
2016-05-14T06:38:47.90-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.Transport.EurekaHttpClient[0]
2016-05-14T06:38:47.90-0600 [APP/0]      OUT       DoGetApplicationsAsync .....
2016-05-14T06:38:47.91-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.DiscoveryClient[0]
2016-05-14T06:38:47.91-0600 [APP/0]      OUT       FetchFullRegistry returned: OK, Applications[Application[Name=FORTUNESERVICE ....
2016-05-14T06:38:47.91-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.DiscoveryClient[0]
2016-05-14T06:38:47.91-0600 [APP/0]      OUT       FetchRegistry succeeded
2016-05-14T06:38:47.99-0600 [APP/0]      OUT verb: Microsoft.AspNet.Hosting.Internal.HostingEngine[4]
2016-05-14T06:38:47.99-0600 [APP/0]      OUT       Hosting starting
2016-05-14T06:38:48.02-0600 [APP/0]      OUT info: Microsoft.Net.Http.Server.WebListener[0]
2016-05-14T06:38:48.02-0600 [APP/0]      OUT       Start
2016-05-14T06:38:48.07-0600 [APP/0]      OUT info: Microsoft.Net.Http.Server.WebListener[0]
2016-05-14T06:38:48.07-0600 [APP/0]      OUT       Listening on prefix: http://*:58442/
2016-05-14T06:38:48.12-0600 [APP/0]      OUT       Hosting started
2016-05-14T06:38:48.12-0600 [APP/0]      OUT verb: Microsoft.AspNet.Hosting.Internal.HostingEngine[5]
2016-05-14T06:38:48.12-0600 [APP/0]      OUT Hosting environment: development
2016-05-14T06:38:48.12-0600 [APP/0]      OUT Now listening on: http://*:58442
2016-05-14T06:38:48.12-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```
At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit http://fortuneui.x.y.z/ to see it!

