# Fortune-Teller-UI - ASP.NET Core Micro-service

ASP.NET Core sample app illustrating how to use the Tracing features found in the Steeltoe Management framework. This sample requires running two additional services in order for it to run properly.

First, a [Spring Cloud Eureka Server](https://cloud.spring.io/spring-cloud-static/Edgware.SR3/multi/multi_spring-cloud-eureka-server.html) for registering micro services. The Fortune-Teller-Service registers the fortuneService with the Eureka server upon startup.  The Fortune-Teller-UI will discover the service using the Steeltoe Eureka client.

Second, a [Zipkin Server](https://zipkin.io/pages/quickstart) for capturing and viewing trace information captured by both Fortune-Teller components.

Follow the instructions in the README for the Fortune-Teller-Service to see how to startup the above two services.

Note: You can run all of this either locally or on CloudFoundry.

## Building & Running - Locally

1. Clone this repository. (i.e. git clone <https://github.com/SteeltoeOSS/Samples>)
1. cd samples/Discovery/src/AspDotNetCore/Fortune-Teller-UI
1. dotnet run -f netcoreapp2.0

## What to expect - Locally

After building and running the app, you should see something like the following:

```bash
$ cd samples/Discovery/src/AspDotNetCore/Fortune-Teller-UI
$ dotnet run -f netcoreapp2.0
Hosting environment: Production
Now listening on: http://*:5555
Application started. Press Ctrl+C to shut down.
```

At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit <http://localhost:5555/> to see it!

Next, to view the distributed traces captured by the Steeltoe Tracing framework, open a browser to <http://localhost:9411/zipkin/>.  Next, go back to the Fortune-Teller-UI and hit refresh a few times to get new fortunes.  Then go back to the Zipkin server UI, and in the `Sort` field select `Newest First`. Then to fetch some traces hit `Find Traces`.  At that point you should see several traces that you can select and view details.

Note: Traces that have 6 spans will be traces of the application obtaining fortunes.  Traces that are a single span are traces captured of the applications interaction with the Eureka Server.

## Pre-requisites - CloudFoundry

To be provided.