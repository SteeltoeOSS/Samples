# Steeltoe Circuit Breaker Sample Applications

This repo tree contains two samples illustrating how to use Steeltoe [Circuit Breakers](https://steeltoe.io/circuit-breakers).

One sample uses ASP.NET Core, the other uses ASP.NET MVC/WebAPI, but both share the following characteristics:

- Fortune Teller Service - a microservice for providing fortunes
- Fortune Teller UI - an MVC application that wraps calls for retrieving fortunes from the back-end service with circuit breakers
- Service discovery via [Spring Cloud Eureka Server](https://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#spring-cloud-eureka-server) (the service registers itself, the UI discovers the service)

The application also illustrates how to use the Hystrix dashboard, both on Cloud Foundry and locally, for monitoring the status of Hystrix commands.

## Building & Running

Each project in each sample contains a Readme for instructions on building and running each app.

---

### See the Official [Steeltoe Circuit Breaker Documentation](https://steeltoe.io/circuit-breakers/docs) for a more information
