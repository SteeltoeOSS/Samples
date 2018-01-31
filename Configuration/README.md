# Steeltoe Configuration Sample Applications

This repo tree contains sample apps illustrating how to use the Steeltoe [Configuration](https://github.com/SteeltoeOSS/Configuration) provider packages.

* src/AspDotNetCore/Simple - ASP.NET Core sample app illustrating how to use [Spring Cloud Config Server](https://cloud.spring.io/spring-cloud-config/) as a configuration source.
* src/AspDotNetCore/SimpleCloudFoundry - ASP.NET Core sample app illustrating how to use [Config Server for Pivotal CloudFoundry](https://docs.pivotal.io/spring-cloud-services/index.html) as a configuration source. It also shows how to access CloudFoundry `VCAP_APPLICATION` and `VCAP_SERVICES` environment variables as configuration data.
* src/AspDotNetCore/CloudFoundry - ASP.NET Core sample app illustrating how to use the Steeltoe [CloudFoundry](https://github.com/SteeltoeOSS/Configuration/tree/master/src/Steeltoe.Extensions.Configuration.CloudFoundry) configuration provider to access CloudFoundry `VCAP_APPLICATION` and `VCAP_SERVICES` environment variables as configuration data.
* src/AspDotNet4/Simple - same as AspDotNetCore/Simple but built for ASP.NET 4.x
* src/AspDotNet4/SimpleCloudFoundry - same as AspDotNetCore/SimpleCloudFoundry, but built for ASP.NET 4.x.
* src/AspDotNet4/AutofacCloudFoundry -same as AspDotNet4/SimpleCloudFoundry, but built using Autofac IOC container.

## Building & Running

See the Readme for instructions on building and running each app.

---

### See the Official [Steeltoe Configuration Documentation](https://steeltoe.io/docs/steeltoe-configuration) for a more in-depth walkthrough of the samples and more detailed information