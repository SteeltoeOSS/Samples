# CloudFoundry JWT Bearer Token Security Sample App

ASP.NET Core Web API sample app illustrating how to make use of the Steeltoe [CloudFoundry JWT Security Provider](https://docs.steeltoe.io/api/v3/security/jwt-authentication.html) for Authentication and Authorization against a CloudFoundry OAuth2 security service (e.g. [Tanzu Single Signon](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service)) using JWT Bearer tokens.

This sample illustrates how you can secure your web api endpoints using JWT Bearer tokens issued by the SSO tile.

> Note: This application is intended to be used in conjunction with the [CloudFoundrySingleSignon][sso] sample app.  You should FIRST get that sample up and running on CloudFoundry and follow these instructions after that.

## Pre-requisites - CloudFoundry

1. [CloudFoundrySingleSignon][sso] already up and running

## Push to CloudFoundry

1. Open your favorite shell
1. login to your Cloud Foundry environment
1. target your org and space `cf target -o myorg -s development`
1. enter the directory for this project `cd samples/Security/src/CloudFoundryJwtAuthentication`
1. `cf push`

The provided manifest(s) will create an app named `jwtauth` and attempt to bind it to `mySSOService`.

> Note: `mySSOService` is created when you follow the instructions for [CloudFoundrySingleSignon][sso].

## What to expect - CloudFoundry

After building and running the app, you should see something like the following in the logs.

To see the logs as you startup and use the app: `cf logs jwtauth`

On a Windows cell, you should see something like this during startup:

```bash
2016-08-05T07:23:02.15-0600 [CELL/0]     OUT Creating container
2016-08-05T07:23:03.81-0600 [CELL/0]     OUT Successfully created container
2016-08-05T07:23:09.07-0600 [APP/0]      OUT Running .\CloudFoundryJwtAuthentication
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Hosting environment: development
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Content root path: C:\containerizer\75E10B9301D2D9B4A8\user\app
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Now listening on: http://*:51217
```

At this point the app is up and running.  To access it, you should use the app: [CloudFoundrySingleSignon][sso].

[sso]: ../CloudFoundrySingleSignon

---

### See the Official [Steeltoe Security Documentation](https://docs.steeltoe.io/api/v3/security/) for a more in-depth walkthrough of the samples and more detailed information
