# Steeltoe Sample Applications
This repo contains several sample applications illustrating how to use the Steeltoe frameworks. 

* [Configuration](https://github.com/SteeltoeOSS/Samples/tree/master/Configuration) - various ASP.NET Core and ASP.NET 4.x samples illustrating how to use the Spring Cloud Config Server and access CloudFoundry `VCAP_*` environment variables as configuration data.
* [Connectors](https://github.com/SteeltoeOSS/Samples/tree/master/Connectors) - several samples illustrating how to use the Steeltoe Connectors for connecting to CloudFoundry services in your ASP.NET Core application. Steeltoe Connectors simplify the coding process of binding to and accessing CloudFoundry based services.
* [Discovery](https://github.com/SteeltoeOSS/Samples/tree/master/Discovery) - ASP.NET Core and ASP.NET 4.x sample apps illustrating how to use the Steeltoe discovery packages for Service discovery in micro services based application.
* [CircuitBreaker](https://github.com/SteeltoeOSS/Samples/tree/master/CircuitBreaker) - ASP.NET Core and ASP.NET 4.x sample apps illustrating how to use the Steeltoe Circuit Breaker packages for building scalable and resilient micro services based application.
* [Security](https://github.com/SteeltoeOSS/Samples/tree/master/Security) - sample apps illustrating how to make use of the Steeltoe CloudFoundry External Security Providers for Authentication and Authorization against a CloudFoundry OAuth2 security service(e.g. UAA Server or Pivotal Single Signon). Also includes a sample illustrating how to make use of a Redis cache for Dataprotection KeyRing storage.
* [MusicStore](https://github.com/SteeltoeOSS/Samples/tree/master/MusicStore) -  a sample app illustrating how to use all of the Steeltoe components together in a ASP.NET Core application. This is a microservies based application built from the ASP.NET Core reference app MusicStore provided by Microsoft.
* [FreddysBBQ](https://github.com/SteeltoeOSS/Samples/tree/master/FreddysBBQ) - a polyglot (i.e. Java and .NET) microservices based sample app illustrating interoperability between Java and .NET based microservices running on CloudFoundry, secured with OAuth2 Security Services and using Spring Cloud Services.

# Branches

All new development is done on the dev branch. More stable versions of the samples can be found on the master branch.

# Documentation

If you are looking for documentation on how to use the Steeltoe components, you can find that [here](https://steeltoe.io/docs/).

# Building & Running

See the Readmes for each sample for instructions on how to build and run.

