# Fortune-Teller-Service - ASP.NET Core Micro-service

ASP.NET Core sample app illustrating how to use the Tracing features found in the Steeltoe Management framework. This sample requires running two additional services in order for it to run properly.

First, a [Spring Cloud Eureka Server](https://cloud.spring.io/spring-cloud-static/Edgware.SR3/multi/multi_spring-cloud-eureka-server.html) for registering micro services. The Fortune-Teller-Service registers the fortuneService with the Eureka server upon startup.  The Fortune-Teller-UI will discover the service using the Steeltoe Eureka client.

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

If you have a running docker environment installed on your system, the you should be able to:

1. docker run -d -p:8761:8761 steeltoeoss/eurekaserver

### Running Zipkin Server Locally

1. Follow the instructions in the [Zipkin Quickstart](https://zipkin.io/pages/quickstart) guide. You will need Java installed to do this.

### Running Zipkin Server Locally - Docker

If you have a running docker environment installed on your system, the you should be able to:

1. docker run -d -p 9411:9411 openzipkin/zipkin

## Building & Running - Locally

1. Clone this repository. (i.e. git clone <https://github.com/SteeltoeOSS/Samples>)
1. cd samples/Discovery/src/AspDotNetCore/Fortune-Teller-Service
1. dotnet run -f netcoreapp2.0

## What to expect - Locally

After building and running the app, you should see something like the following:

```bash
$ cd samples/Discovery/src/AspDotNetCore/Fortune-Teller-Service
$ dotnet run -f netcoreapp2.0
info: Microsoft.Data.Entity.Storage.Internal.InMemoryStore[1]
      Saved 50 entities to in-memory store.
Hosting environment: Production
Now listening on: http://*:5000
Application started. Press Ctrl+C to shut down.
```

At this point the Fortune Teller Service is up and running and ready for the Fortune Teller UI to ask for fortunes.  Go to the Fortune-Teller-UI directory for details on how to start it up.

## Pre-requisites - CloudFoundry

To be provided.