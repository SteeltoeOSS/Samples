# Steeltoe Security Sample Applications

This section of the Samples repository contains applications that use the [Steeltoe Security Packages](https://docs.steeltoe.io/api/v3/security/) for authentication, authorization, and data protection.

## ASP.NET Core Samples

* [AuthWeb](src/AuthWeb/README.md) and [AuthApi](src/AuthApi/README.md) - authenticate and authorize with OpenID Connect and JWT Bearer tokens using [Single Sign-On for VMware Tanzu Application Service](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service)) and client certificates.
* [RedisDataProtection](src/RedisDataProtection/README.md) - use Redis provisioned on Cloud Foundry as a DataProtection Key Store.  Sample illustrates sharing encrypted data stored in an ASP.NET session across multiple instances of an application.

---

See the Official [Steeltoe Security Documentation](https://docs.steeltoe.io/api/v3/security/) for more detailed information
