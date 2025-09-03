# Steeltoe MusicStore Sample Application

This repo tree contains a sample app illustrating how to use all of the Steeltoe components together in a ASP.NET Core application. This application is based on the old ASP.NET Core reference app MusicStore, previously provided by Microsoft.

In creating this application, we took the Microsoft reference application and broke it up into multiple independent services:

* MusicStoreUI - provides the UI to the MusicStore application and all of its services.
* MusicStoreService - provides a RESTful API to the MusicStore and its backend Music database.
* OrderService - provides a RESTful API for Order processing service and its backend Order database.
* ShoppingCartService - provides a RESTful api to a ShoppingCart service and its backend ShoppingCart database.

> Note: The OrderService and ShoppingCartService are independent from the Music application and could be used in any other application requiring those basic services.

This application makes use of the following Steeltoe components:

* Spring Cloud Config Server Client for centralized application configuration
* Spring Cloud Eureka Server Client for service discovery
* Steeltoe Connector for connecting to MySql using EFCore
* Steeltoe CircuitBreaker to help prevent cascading failures from lower level service failures
* Steeltoe Management for enabling management actuator endpoints
* Spring Boot Admin for interacting with management actuators and viewing detailed service status
* Steeltoe Redis Connector to connect to a Redis cache for Session storage. Note: This is required if you want to scale the MusicStoreUI component to multiple instances.
* Steeltoe Redis DataProtection provider to the cause the DataProtection KeyRing to be stored in a Redis cache. Note: This is also required if you want to scale the MusicStoreUI component to multiple instances.
* Optionally uses Hystrix Dashboard for monitoring Circuit Breakers

Usage of Redis for Session storage DataProtection KeyRing storage is controlled by the environment variable `USE_REDIS_CACHE` for MusicStoreUI. This variable is not set if you run the project directly or with Docker Compose.

## Getting Started

* Clone the Samples repo. (i.e.  git clone <https://github.com/SteeltoeOSS/Samples>)

### Using Docker compose

1. `cd Samples/MusicStore`
1. `docker-compose build`
1. `docker-compose up`

## Cloud Foundry

See [the Cloud Foundry readme](./README-CloudFoundry.md)

## Other Platforms

Review the resources in the [deployment folder](./deployment) for templates and scripts for deploying to other platforms like Kubernetes
