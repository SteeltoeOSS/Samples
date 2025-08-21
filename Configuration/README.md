# Steeltoe Configuration Sample Applications

This repo tree contains several samples illustrating how to use the Steeltoe [configuration providers](https://steeltoe.io/docs/v3/configuration/index.html).

* src/CloudFoundry - shows how to use the [Cloud Foundry configuration provider](https://steeltoe.io/docs/v3/configuration/cloud-foundry-provider.html) to access `VCAP_APPLICATION` and `VCAP_SERVICES` environment variables as configuration data.
* src/Kubernetes - shows how to use the [configuration provider for Kubernetes ConfigMaps and Secrets](https://steeltoe.io/docs/v3/configuration/kubernetes-providers.html).
* src/Placeholder - shows how to use the [placeholder configuration provider](https://steeltoe.io/docs/v3/configuration/placeholder-provider.html).
* src/RandomValue - shows how to use the [random value configuration provider](https://steeltoe.io/docs/v3/configuration/random-value-provider).
* src/Simple - shows how to use [Spring Cloud Config Server](https://spring.io/projects/spring-cloud-config) as a configuration source.
* src/SimpleCloudFoundry - shows how to use [Config Server for Tanzu Platform](https://techdocs.broadcom.com/us/en/vmware-tanzu/spring/spring-cloud-services-for-cloud-foundry/3-3/scs-tanzu/index.html) as a configuration source. It also shows how to access CloudFoundry `VCAP_APPLICATION` and `VCAP_SERVICES` environment variables as configuration data.

## Building & Running

See the Readme for instructions on building and running each app.

---

### See the Official [Steeltoe Configuration Documentation](https://steeltoe.io/docs/v3/configuration/index.html) for a more in-depth walkthrough of the samples and more detailed information
