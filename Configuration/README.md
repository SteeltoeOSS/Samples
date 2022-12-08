# Steeltoe Configuration Sample Applications

This repo tree contains several samples illustrating how to use the Steeltoe [Configuration](https://steeltoe.io/app-configuration/get-started) providers.

* src/Simple - ASP.NET Core sample app illustrating how to use [Spring Cloud Config Server](https://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#_spring_cloud_config_server) as a configuration source.
* src/SimpleCloudFoundry - ASP.NET Core sample app illustrating how to use [Config Server for Pivotal CloudFoundry](https://docs.pivotal.io/spring-cloud-services/index.html) as a configuration source. It also shows how to access CloudFoundry `VCAP_APPLICATION` and `VCAP_SERVICES` environment variables as configuration data.
* src/CloudFoundry - ASP.NET Core sample app illustrating how to use the Steeltoe [CloudFoundry](https://github.com/SteeltoeOSS/Configuration/tree/main/src/Steeltoe.Extensions.Configuration.CloudFoundry) configuration provider to access CloudFoundry `VCAP_APPLICATION` and `VCAP_SERVICES` environment variables as configuration data.

## Building & Running

See the Readme for instructions on building and running each app.

---

### See the Official [Steeltoe Configuration Documentation](https://steeltoe.io/app-configuration) for a more in-depth walkthrough of the samples and more detailed information
