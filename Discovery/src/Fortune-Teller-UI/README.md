# Fortune-Teller-UI - ASP.NET 5 MVC App
ASP.NET 5 sample app illustrating how to use [Spring Cloud Eureka Server](http://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#spring-cloud-eureka-server) for discovering micro services. The Fortune-Teller-UI attempts to locate the fortuneService in the Eureka server and uses it to get your fortune.

# Pre-requisites

This sample assumes that there is a running Spring Cloud Eureka Server on your machine. To make this happen

1. Install Java 8 JDK.
2. Install Maven 3.x.
3. Clone the Spring Cloud Samples Eureka repository. (https://github.com/spring-cloud-samples/eureka.git)
4. Go to the eureka server directory (`eureka`) and fire it up with `mvn spring-boot:run`
5. This sample will default to looking for its eurka server on http://localhost:8761/eureka, so it should all connect.

# Building & Running

1. Clone this repo. (i.e. git clone https://github.com/SteelToeOSS/Samples)
2. cd samples/Discovery/src/Fortune-Teller-UI
3. Install DNX 1.0.0-rc1-final/update1. Install either the coreclr and/or clr runtimes. 
4. Add a DNX runtime to your path. (e.g. dnvm use 1.0.0-rc1-update1 -r clr)
5. dnu restore
6. dnx web

# What to expect
After building and running the app, you should see something like the following:
```
$ cd samples/Configuration/src/Simple
$ dnx web
Hosting environment: Production
Now listening on: http://localhost:5555
Application started. Press Ctrl+C to shut down.
```
At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit http://localhost:5555/ to see it!