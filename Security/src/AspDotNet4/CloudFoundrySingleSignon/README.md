# CloudFoundry Single Signon Security Sample App

Legacy ASP.NET sample app using the Steeltoe [CloudFoundry External Security Provider](https://github.com/SteeltoeOSS/Security) for Authentication and Authorization against a CloudFoundry OAuth2 security service (e.g. [UAA Server](https://github.com/cloudfoundry/uaa) or [Pivotal Single Signon Service](https://docs.pivotal.io/p-identity/)).

> See also: instructions for [using the SSO Tile with this sample](README-SSO.md)

## Pre-requisites

1. Cloud Foundry instance
1. Windows support on Cloud Foundry
1. [CloudFoundry UAA Command Line Client](https://github.com/cloudfoundry/cf-uaac) installed

### Create OAuth2 Service Instance on CloudFoundry

This sample requires an OAuth2 service in your org/space. These instructions will use the [UAA Server](https://github.com/cloudfoundry/uaa) as the OAuth2 service.

### Target your Environment with UAA Command Line Tools

Before creating the OAuth2 service instance, we need to use the UAA command line tool to establish some security credentials for our sample app. Target your UAA server:

1. uaac target uaa.`YOUR-CLOUDFOUNDRY-SYSTEM-DOMAIN` (e.g. `uaac target uaa.system.testcloud.com`)

Next, authenticate and obtain an access token for the `admin client` from the UAA server so that we can add our new application/user credentials. You will need the `Admin Client Secret` for your installation of CloudFoundry for this step. If you are using Pivotal CloudFoundry (PCF), you can obtain the secret from the `Ops Manager/Pivotal Application Service` credentials page under the `UAA` section.  Look for `Admin Client Credentials` and then use it as follows:

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
uaac client add myTestApp --scope cloud_controller.read,cloud_controller_service_permissions.read,openid,testgroup --authorized_grant_types authorization_code,refresh_token --authorities uaa.resource --redirect_uri http://single-signon.`YOUR-CLOUDFOUNDRY-APP-DOMAIN`/signin-oidc --autoapprove cloud_controller.read,cloud_controller_service_permissions.read,openid,testgroup --secret myTestApp
```

### Add User-Provided Service with OAuth Details

Last, we will create a user-provide service that includes the appropriate UAA server configuration data. Use the sample below to pass the parameters directly to the `cf cups` command, replacing `<YOUR-CLOUDFOUNDRY-SYSTEM-DOMAIN>` with your domain.

```bash
cf target -o myorg -s development
cf cups myOAuthService -p "{\"client_id\": \"myTestApp\",\"client_secret\": \"myTestApp\",\"uri\": \"uaa://login.<YOUR-CLOUDFOUNDRY-SYSTEM-DOMAIN>\"}"
```

## Publish App & Push to CloudFoundry

1. Open Samples\Security\src\AspDotNet4\4x-Security.sln in Visual Studio
1. Right click on the CloudFoundrySingleSignon project, select "Publish"
1. Use the included `FolderProfile` to publish to `bin/Debug/net461/win10-x64/publish`
1. Use the CF CLI to push the app

```bash
> cf target -o myorg -s development
> cd samples/Security/src/AspDotNet4/CloudFoundrySingleSignon
> cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish
```

> Note: The provided manifest(s) will create an app named `single-signon` and attempt to bind it to the user-provided service `myOAuthService`.

## What to expect - CloudFoundry

After pushing the app, you should see something like the following in the logs:

```bash
2018-05-04T10:18:00.874-05:00 [API/0] [OUT] Starting app with guid 737c9bce-3262-4434-91d2-563ff9871d66
2018-05-04T10:18:01.032-05:00 [CELL/0] [OUT] Creating container
2018-05-04T10:18:01.611-05:00 [CELL/0] [OUT] Successfully destroyed container
2018-05-04T10:18:01.965-05:00 [CELL/0] [OUT] Successfully created container
2018-05-04T10:18:04.141-05:00 [CELL/0] [OUT] Starting health monitoring of container
2018-05-04T10:18:06.015-05:00 [APP/PROC/WEB/0] [OUT] Server Started for b096336d-5b38-43a2-815b-7c90dc67d46a
2018-05-04T10:18:08.928-05:00 [CELL/0] [OUT] Container became healthy
```

At this point the app is up and running.  You can access it at <http://single-signon.`YOUR-CLOUDFOUNDRY-APP-DOMAIN`/>.

> Note: To see the logs as the app runs, execute this command: `cf logs single-signon`

On the app's menu, click on the `Log in` menu item and you should be redirected to the CloudFoundry login page. Enter `dave` and `Password1!`, or whatever name/password you used above,  and you should be authenticated and redirected back to the single-signon home page.

Two of the endpoints in the `HomeController` have Authorization policies applied:

```csharp
[CustomClaimsAuthorize("testgroup")]
public ActionResult TestGroup()
{
    ViewBag.Title = "Steeltoe Legacy ASP.NET Security Samples";
    ViewBag.Message = "Congratulations, you have access to 'testgroup'";
    return View("Index");
}

[CustomClaimsAuthorize("testgroup1")]
public ActionResult TestGroup1()
{
    return View("Index");
}
```

If you try and access the `TestGroup` menu item you should see the `TestGroup` page as user `dave` is a member of that group and is authorized to access the endpoint.

If you try and access the `TestGroup1` menu item you should see `Access Denied, Insufficent permissions` as `dave` is not a member of `testgroup1` and therefore can not access the end point.

If you access the `JWT Sample` or `WCF Sample` menu items, you will find the app will attempt to invoke secured endpoints in the other Security sample apps [CloudFoundryJwtAuthentication][jwt] and [CloudFoundryWCF][wcf]. In order for this to be functional you must first push the [CloudFoundryJwtAuthentication][jwt] and/or [CloudFoundryWCF][wcf] samples using the same instructions found on this page, adjusted to match project names and locations.

[jwt]: ../CloudFoundryJwtAuthentication
[wcf]: ../CloudFoundryWcf