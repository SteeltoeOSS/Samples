# Steeltoe Security Sample Applications

This section of the Samples repository contains applications that use the [Steeltoe Security Packages](https://docs.steeltoe.io/api/v3/security/) for authentication, authorization, data protection, and credential management.

## ASP.NET Core Samples

* [CloudFoundrySingleSignon](src/CloudFoundrySingleSignon/README.md) - ASP.NET Core sample showing how to Authenticate and Authorize against [Tanzu Single Signon](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service)).
* CloudFoundryJwtAuthentication - ASP.NET Core Web API sample app illustrating how to Authenticate and Authorize against [Tanzu Single Signon](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service)) using JWT Bearer tokens.
* CredHubDemo - ASP.NET Core app showing how to use the Steeltoe CredHub Client to interact with a [CredHub Server](https://github.com/cloudfoundry/credhub) for accessing or storing credentials.
* RedisDataProtectionKeyStore - ASP.NET Core sample app illustrating how to use a Redis CloudFoundry service as a DataProtection Key Store.  Sample illustrates sharing encrypted data stored in a Session across multiple instances of an application.

---

### See the Official [Steeltoe Security Documentation](https://docs.steeltoe.io/api/v3/security/) for more detailed information
