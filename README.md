# Steeltoe Sample Applications

This repository contains a variety of sample applications illustrating how to use the Steeltoe frameworks:

* [Configuration](Configuration) - samples using the Spring Cloud Config Server and other Steeltoe configuration providers.
* [Discovery](Discovery) - sampless using Steeltoe Service Discovery microservices-based application.
* [Management](Management) - samples using the Steeltoe Management packages for adding Management REST endpoints to your application as well as adding Distributed Tracing support.
* [Connectors](Connectors) - samples using the Steeltoe Connectors for connecting to backing services. Steeltoe Connectors simplify the coding process of binding to and accessing Cloud Foundry based services.
* [CircuitBreaker](CircuitBreaker) - samples using the Steeltoe Circuit Breaker packages for building scalable and resilient microservices-based application.
* [Security](Security) - samples using the Steeltoe Security packages for Authentication and Authorization with Cloud Foundry auth services, using a Redis cache for DataProtection KeyRing storage and for interacting with CredHub.
* [MusicStore](MusicStore) - a sample that uses all of the Steeltoe components together in a microservices-based ASP.NET Core application. Adapted from the ASP.NET Core reference app: [MusicStore by Microsoft](https://github.com/aspnet/AspNetCore/tree/main/src/MusicStore).
* [FreddysBBQ](FreddysBBQ) - a polyglot (Java and .NET) microservices-based sample demonstrating interoperability between Java and .NET on CloudFoundry, secured with OAuth2 Security Services and using Spring Cloud Services.

## Branches

All new development is done on the dev branch. More stable versions of the samples can be found on the main branch.

## Documentation

If you are looking for documentation on how to use the Steeltoe components, you can find that [here](https://steeltoe.io/docs/).

## Building & Running

See the Readmes for each sample for instructions on how to build and run.
