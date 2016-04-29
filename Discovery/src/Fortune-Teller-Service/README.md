# Fortune-Teller-Service - ASP.NET 5 Microservice
ASP.NET 5 sample app illustrating how to use [Spring Cloud Eureka Server](http://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#spring-cloud-eureka-server) for registering micro services. The Fortune-Teller-Service registers the fortuneService with the Eureka server upon startup.

# Pre-requisites

This sample assumes that there is a running Spring Cloud Eureka Server on your machine. To make this happen

1. Install Java 8 JDK.
2. Install Maven 3.x.
3. Clone the Spring Cloud Samples Eureka repository. (https://github.com/spring-cloud-samples/eureka.git)
4. Go to the eureka server directory (`eureka`) and fire it up with `mvn spring-boot:run`
5. This sample will default to looking for its eurka server on http://localhost:8761/eureka, so it should all connect.

# Building & Running

1. Clone this repo. (i.e. git clone https://github.com/SteelToeOSS/Samples)
2. cd samples/Discovery/src/Fortune-Teller-Service
3. Install DNX 1.0.0-rc1-final/update1. Install either the coreclr and/or clr runtimes. 
4. Add a DNX runtime to your path. (e.g. dnvm use 1.0.0-rc1-update1 -r clr)
5. dnu restore
6. dnx web

# What to expect
After building and running the app, you should see something like the following:
```
$ cd samples/Configuration/src/Simple
$ dnx web
info: Microsoft.Data.Entity.Storage.Internal.InMemoryStore[1]
      Saved 50 entities to in-memory store.
Hosting environment: Production
Now listening on: http://*:5000
Application started. Press Ctrl+C to shut down.
```
At this point the Fortune Teller Service is up and running and ready for the [Fortune Teller UI]() to ask for fortunes.
