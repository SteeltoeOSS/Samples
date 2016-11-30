# Steeltoe Security Sample Applications
This repo tree contains  sample apps illustrating how to make use of the Steeltoe [CloudFoundry External Security Provider](https://github.com/SteeltoeOSS/Security) for Authentication and Authorization.
* CloudFoundrySingleSignon - ASP.NET Core sample app illustrating how to Authenticate and Authorize against a CloudFoundry OAuth2 security service (e.g. [UAA Server](https://github.com/cloudfoundry/uaa) or [Pivotal Single Signon](https://docs.pivotal.io/p-identity/)).
* CloudFoundryJwtAuthentication - ASP.NET Core Web API sample app illustrating how to Authenticate and Authorize against a CloudFoundry OAuth2 security service (e.g. [UAA Server](https://github.com/cloudfoundry/uaa) or [Pivotal Single Signon](https://docs.pivotal.io/p-identity/)) using JWT Bearer tokens.
* RedisDataProtectionKeyStore - ASP.NET Core sample app illustrating how to use a Redis CloudFoundry service as a DataProtection Key Store.  Sample illustrates sharing encrypted data stored in a Session across multiple instances of an application.

# Building & Running
See the Readme for instructions on building and running each app.
