# CloudFoundry Single Signon Sample App (with SSO Tile)

ASP.NET Core sample app illustrating how to make use of the Steeltoe [CloudFoundry External Security Provider](https://github.com/SteeltoeOSS/Security) for Authentication and Authorization against a CloudFoundry OAuth2 security service. These instructions are for [Pivotal Single Signon Service](https://docs.pivotal.io/p-identity/), instructions for using UAA directly are [in the other README](README.md).

## Pre-requisites - CloudFoundry

1. Cloud Foundry instance
1. Windows support on Cloud Foundry (OPTIONAL)
1. [.NET Core SDK](https://www.microsoft.com/net/download) installed
1. [CloudFoundry UAA Command Line Client](https://github.com/cloudfoundry/cf-uaac) installed
1. [SSO Tile](https://docs.pivotal.io/p-identity/installation.html) installed

### Create a Service Plan

In order to use the SSO tile in an application, you must first [create a service plan](https://docs.pivotal.io/p-identity/1-5/manage-service-plans.html#create-svc-plan). The rest of this page assumes the `Auth Domain` for your SSO serice plan is `auth`.

> Note: use the command `cf marketplace` to see your SSO plan once it has been created.

### Create a Service Instance

In order to bind SSO configuration information to our application, create a service instance: `cf create-service p-identity auth mySSOService`

### Execute Script to Configure new User and Group

Next, locate the sso-setup script you'd like to execute (.cmd/.sh) from the scripts folder in this repository. Update the variables to suit your environment - You will need the `Admin Client Secret` for your installation of CloudFoundry for this step. If you are using Pivotal CloudFoundry (PCF), you can obtain the secret from the `Ops Manager/Pivotal Application Service` credentials page under the `UAA` section.  Look for `Admin Client Credentials`.

### Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
1. cd samples/Security/src/CloudFoundrySingleSignon
1. dotnet restore --configfile nuget.config
1. Publish app to a directory, specifying the desired framework and runtime:
    * `dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64`
    * `dotnet publish -f netcoreapp2.1 -r win10-x64`
1. Push the app using the appropriate manifest:
    * `cf push -f manifest-sso.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish`
    * `cf push -f manifest-windows-sso.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish`

> Note: The provided manifest(s) will create an app named `single-signon` and attempt to bind it to the SSO service `mySSOService`.

### Configure SSO RedirectUri and Scope access

For the application to access group information and handle login redirects correctly, you must configure two properties in the `sso` service dashboard. In order to access the `sso` dashboard, run the following command and go to the URL listed in `Dashboard` property:

```bash
$ cf service mySSOService

Service instance: mySSOService
Service: p-identity
bound apps: single-signon
Tags:
Plan: auth
Description: Single Sign-On as a Service
Documentation url: http://docs.pivotal.io/p-identity/index.html
Dashboard: https://p-identity.mypcf.example.com/dashboard/identity-zones/{ZONE_GUID}/instances/{INSTANCE_GUID}/
...
```

On the dashboard, under `Apps`:

1. Select the `single-signon` app.
1. Click "Select Scopes" and add the `testgroup` scope
1. Add the URI `http://single-signon.<YOUR-CLOUDFOUNDRY-SYSTEM-DOMAIN>/signin-cloudfoundry` under "Auth Redirect URIs"
1. Click the "Save Config" button

## What to expect - CloudFoundry

After pushing the app, you should see something like the following in the logs:

```bash
2016-08-05T07:23:02.15-0600 [CELL/0]     OUT Creating container
2016-08-05T07:23:03.81-0600 [CELL/0]     OUT Successfully created container
2016-08-05T07:23:09.07-0600 [APP/0]      OUT Running .\CloudFoundrySingleSignon
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Hosting environment: development
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Content root path: C:\containerizer\75E10B9301D2D9B4A8\user\app
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Now listening on: http://*:51217
```

At this point the app is up and running.  You can access it at <http://single-signon.`YOUR-CLOUDFOUNDRY-APP-DOMAIN`/>.

> Note: To see the logs as the app runs, execute this command: `cf logs single-signon`

On the apps menu, click on the `Log in` menu item and you should be redirected to the CloudFoundry login page. Enter `dave` and `Password1!`, or whatever name/password you used above,  and you should be authenticated and redirected back to the single-signon home page.

Two of the endpoints in the `HomeController` have Authorization policys applied:

```csharp
[Authorize(Policy = "testgroup")]
public IActionResult About()
{
    ViewData["Message"] = "Your application description page.";

    return View();
}


[Authorize(Policy = "testgroup1")]
public IActionResult Contact()
{
    ViewData["Message"] = "Your contact page.";

    return View();
}
```

If you try and access the `About` menu item you should see the `About` page as user `dave` is a member of that group and is authorized to access the end point.

If you try and access the `Contact` menu item you should see `Access Denied, Insufficent permissions` as `dave` is not a member of the `testgroup1` and therefore can not access the end point.

If you access the `InvokeJwtSample` menu item, you will find the app will attempt to invoke a secured endpoint in a second Security sample app [CloudFoundryJwtAuthentication][jwt]. In order for this to be functional you must first push the [CloudFoundryJwtAuthentication][jwt] sample using the Readme instructions.

Once you have [CloudFoundryJwtAuthentication][jwt] up and running, then if you access the `InvokeJwtSample` menu item when you are logged in, you should see some `values` returned from the [CloudFoundryJwtAuthentication][jwt] app.  If you are logged out, then you will see a `401 (Unauthorized)` message.

[jwt]: ../CloudFoundryJwtAuthentication
