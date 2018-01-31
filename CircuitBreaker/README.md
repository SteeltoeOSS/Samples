# Steeltoe Circuit Breaker Sample Applications

This repo tree contains sample apps illustrating how to use the Steeltoe [CircuitBreaker](https://github.com/SteeltoeOSS/CircuitBreaker) packages.

* src/AspDotNetCore/Fortune-Teller/Fortune-Teller-Service - ASP.NET Core microservice illustrating how to use [Spring Cloud Eureka Server](http://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#spring-cloud-eureka-server) for service registration.
* src/AspDotNetCore/Fortune-Teller/Fortune-Teller-UI - ASP.NET Core MVC app illustrating how to use [Spring Cloud Eureka Server](http://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#spring-cloud-eureka-server) for service discovery and [Spring Cloud Hystrix](http://cloud.spring.io/spring-cloud-static/Dalston.SR1/#_circuit_breaker_hystrix_clients) commands for accessing the Fortune Service.  The application also illustrates how to use the Hystrix dashboard, both on Cloud Foundry and locally, for monitoring the status of Hystrix commands.
* src/AspDotNet4/Fortune-Teller/Fortune-Teller-Service4 -same as AspDotNetCore/Fortune-Teller-Service but built for ASP.NET 4.x and using Autofac IOC container
* src/AspDotNetCore/Fortune-Teller/Fortune-Teller-UI - same as AspDotNetCore/Fortune-Teller-UI but built for ASP.NET 4.x and using Autofac IOC container

## Building & Running

See the Readme for instructions on building and running each app.  Instructions for running the apps both locally and on CloudFoundry are provided.

---

### See the Official [Steeltoe Circuit Breaker Documentation](https://steeltoe.io/docs/steeltoe-circuitbreaker) for a more in-depth walkthrough of the samples and more detailed information