# CloudFoundry Single Signon Security Sample App

ASP.NET Core sample app illustrating how to make use of the Steeltoe [CloudFoundry External Security Provider](https://github.com/SteeltoeOSS/Security) for Authentication and Authorization against a CloudFoundry OAuth2 security service (e.g. [UAA Server](https://github.com/cloudfoundry/uaa) or [Pivotal Single Signon Service](https://docs.pivotal.io/p-identity/)).

> NOTE: For simplicity, we've moved the [instructions for utilizing the SSO Tile](README-SSO.md) with this sample

## Pre-requisites

1. Cloud Foundry instance
1. Windows support on Cloud Foundry (OPTIONAL)
1. [.NET Core SDK](https://www.microsoft.com/net/download) installed
1. [CloudFoundry UAA Command Line Client](https://github.com/cloudfoundry/cf-uaac) installed

### Create OAuth2 Service Instance on CloudFoundry

This sample requires an OAuth2 service in your org/space. These instructions will use the [UAA Server](https://github.com/cloudfoundry/uaa) as the OAuth2 service.

### Target your Environment with UAA Command Line Tools

Before creating the OAuth2 service instance, we need to use the UAA command line tool to establish some security credentials for our sample app. Target your UAA server:

1. uaac target uaa.`YOUR-CLOUDFOUNDRY-SYSTEM-DOMAIN` (e.g. `uaac target uaa.system.testcloud.com`)

Next, authenticate and obtain an access token for the `admin client` from the UAA server so that we can add our new application/user credentials. You will need the `Admin Client Secret` for your installation of CloudFoundry for this step. If you are using Pivotal CloudFoundry (PCF), you can obtain the secret from the `Ops Manager/Elastic Runtime` credentials page under the `UAA` section.  Look for `Admin Client Credentials` and then use it as follows:

1. uaac token client get admin -s `ADMIN_CLIENT_SECRET`

> Note: To see the token that was retrieved, run the command `uaac contexts`

### Add User and Group

After authenticating, add a new `user` and `group` to the UAA Server database. Do NOT change the group name: `testgroup` as it is used for policy based authorization in the sample application. Feel free to change the username and password to anything you would like.

1. uaac group add testgroup
1. uaac user add dave --given_name Dave --family_name Tillman --emails dave@testcloud.com --password Password1!
1. uaac member add testgroup dave

### Add New Client for our App

After adding the user and group, we are ready to add our application as a new client to the UAA server. This step will establish our application's credentials and allow it to interact with the UAA server. Use the line below once you have replaced the `YOUR-CLOUDFOUNDRY-APP-DOMAIN` with the domain used by your cloud foundry instance.

```bash
uaac client add myTestApp --scope cloud_controller.read,cloud_controller_service_permissions.read,openid,testgroup --authorized_grant_types authorization_code,refresh_token --authorities uaa.resource --redirect_uri http://single-signon.`YOUR-CLOUDFOUNDRY-APP-DOMAIN`/signin-cloudfoundry --autoapprove cloud_controller.read,cloud_controller_service_permissions.read,openid,testgroup --secret myTestApp
```

### Add User-Provided Service with OAuth Details

Last, we will create a user-provide service that includes the appropriate UAA server configuration data. Use the sample below to pass the parameters directly to the `cf cups` command, replacing `<YOUR-CLOUDFOUNDRY-SYSTEM-DOMAIN>` with your domain.

```bash
cf target -o myorg -s development
cf cups myOAuthService -p "{\"client_id\": \"myTestApp\",\"client_secret\": \"myTestApp\",\"uri\": \"uaa://login.<YOUR-CLOUDFOUNDRY-SYSTEM-DOMAIN>\"}"
```

## Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
1. cd samples/Security/src/CloudFoundrySingleSignon
1. dotnet restore --configfile nuget.config
1. Publish app to a directory selecting the framework and runtime you want to run on.
    * `dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64`
    * `dotnet publish -f netcoreapp2.1 -r win10-x64`
1. Push the app using the appropriate manifest.
    * `cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish`
    * `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish`

> Note: The provided manifest(s) will create an app named `single-signon` and attempt to bind it to the user-provided service `myOAuthService`.

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

On the app's menu, click on the `Log in` menu item and you should be redirected to the CloudFoundry login page. Enter `dave` and `Password1!`, or whatever name/password you used above,  and you should be authenticated and redirected back to the single-signon home page.

Two of the endpoints in the `HomeController` have Authorization policies applied:

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

---

### See the Official [Steeltoe Security Documentation](https://steeltoe.io/docs/steeltoe-security) for a more in-depth walkthrough of the samples and more detailed information