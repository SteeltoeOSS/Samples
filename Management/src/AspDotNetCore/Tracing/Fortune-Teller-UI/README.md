# Fortune-Teller-UI - ASP.NET Core Microservice

ASP.NET Core sample app illustrating how to use the Tracing features found in the Steeltoe Management framework. This sample requires running two additional services in order for it to run properly.

First, a [Spring Cloud Eureka Server](http://cloud.spring.io/spring-cloud-static/Edgware.SR3/multi/multi_spring-cloud-eureka-server.html) for registering micro services. The Fortune-Teller-Service registers the fortuneService with the Eureka server upon startup.  The Fortune-Teller-UI will discover the service using the Steeltoe Eureka client.

Second, a [Zipkin Server](https://zipkin.io/pages/quickstart) for capturing and viewing trace information captured by both Fortune-Teller components.

Follow the instructions in the README for the Fortune-Teller-Service to see how to startup the above two services.

Note: You can run all of this either locally or on CloudFoundry.

## Building & Running - Locally

1. Clone this repository. (i.e. git clone <https://github.com/SteeltoeOSS/Samples>)
1. cd samples/Discovery/src/AspDotNetCore/Fortune-Teller-UI
1. dotnet run -f netcoreapp2.1

## What to expect - Locally

After building and running the app, you should see something like the following:

```bash
$ cd samples/Discovery/src/AspDotNetCore/Fortune-Teller-UI
$ dotnet run -f netcoreapp2.1
Hosting environment: Production
Now listening on: http://*:5555
Application started. Press Ctrl+C to shut down.
```

At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit <http://localhost:5555/> to see it!

Next, to view the distributed traces captured by the Steeltoe Tracing framework, open a browser to <http://localhost:9411/zipkin/>.  Next, go back to the Fortune-Teller-UI and hit refresh a few times to get new fortunes.  Then go back to the Zipkin server UI, and in the `Sort` field select `Newest First`. Then to fetch some traces hit `Find Traces`.  At that point you should see several traces that you can select and view details.

Note: Traces that have 6 spans will be traces of the application obtaining fortunes.  Traces that are a single span are traces captured of the applications interaction with the Eureka Server.

## Pre-requisites - CloudFoundry

Before proceeding with the steps below, make sure you have completed the steps to get the Fortune-Teller-Service up and running on Cloud Foundry.

## Configure Zipkin Server Endpoint in Fortune-Teller-UI

1. Open `appsettings.json` and modify the `management:tracing:exporter:zipkin:endpoint` configuration setting to match the endpoint of the Zipkin server deployed to Cloud Foundry when you started the Fortune-Teller-Service.  (e.g. `http://zipkin-server.cfapps.io/api/v2/spans`)

## Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
1. cd samples/Management/src/AspDotNetCore/Tracing/Fortune-Teller-UI
1. Publish app to a directory selecting the framework and runtime you want to run on. (e.g. `dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64`)
1. Push the app using the appropriate manifest. (e.g. `cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish` or `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish`)

## What to expect - CloudFoundry

After building and running the app, you should see something like the following in the logs.

To see the logs as you startup and use the app: `cf logs fortuneui`

On a Windows cell, you should see something like this during startup:

```bash
2016-05-14T06:38:21.67-0600 [CELL/0]     OUT Successfully created container
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

At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit <http://fortuneui.x.y.z/> to see it!

Next, to view the distributed traces captured by the Steeltoe Tracing framework, open a browser to <http://zipkin-server.cfapps.io/zipkin/>.  Next, go back to the Fortune-Teller-UI and hit refresh a few times to get new fortunes.  Then go back to the Zipkin server UI, and in the `Sort` field select `Newest First`. Then to fetch some traces hit `Find Traces`.  At that point you should see several traces that you can select and view details.

Note: Traces that have 6 spans will be traces of the application obtaining fortunes.  Traces that are a single span are traces captured of the applications interaction with the Eureka Server.