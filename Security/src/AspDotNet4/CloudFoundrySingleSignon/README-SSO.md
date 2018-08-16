# CloudFoundry Single Signon Sample App (with SSO Tile)

Legacy ASP.NET sample app illustrating how to make use of the Steeltoe [CloudFoundry External Security Provider](https://github.com/SteeltoeOSS/Security) for Authentication and Authorization against a CloudFoundry OAuth2 security service. These instructions are for [Pivotal Single Signon Service](https://docs.pivotal.io/p-identity/), instructions for using UAA directly are [in the other README](README.md).

## Pre-requisites - CloudFoundry

1. Cloud Foundry instance
1. Windows support on Cloud Foundry
1. [CloudFoundry UAA Command Line Client](https://github.com/cloudfoundry/cf-uaac) installed
1. [SSO Tile](https://docs.pivotal.io/p-identity/installation.html) installed

### Create a Service Plan

In order to use the SSO tile in an application, you must first [create a service plan](https://docs.pivotal.io/p-identity/1-5/manage-service-plans.html#create-svc-plan). The rest of this page assumes the `Auth Domain` for your SSO serice plan is `auth`.

> Note: use the command `cf marketplace` to see your SSO plan once it has been created.

### Create a Service Instance

In order to bind SSO configuration information to our application, create a service instance: `cf create-service p-identity auth myOAuthService`

### Execute Script to Configure new User and Group

Next, locate the sso-setup script you'd like to execute (.cmd/.sh) from the scripts folder in this repository. Update the variables to suit your environment - You will need the `Admin Client Secret` for your installation of CloudFoundry for this step. If you are using Pivotal CloudFoundry (PCF), you can obtain the secret from the `Ops Manager/Elastic Runtime` credentials page under the `UAA` section.  Look for `Admin Client Credentials`.

### Publish App & Push to CloudFoundry

1. Open Samples\Security\src\AspDotNet4\4x-Security.sln in Visual Studio
1. Right click on the CloudFoundrySingleSignon project, select "Publish"
1. Use the included `FolderProfile` to publish to `bin/Debug/net461/win10-x64/publish`
1. Use the CF CLI to push the app

```bash
> cf target -o myorg -s development
> cd samples/Security/src/AspNet4/CloudFoundrySingleSignon
> cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish
```

> Note: The provided manifest(s) will create an app named `single-signon` and attempt to bind it to the SSO service `myOAuthService`.

### Configure SSO RedirectUri and Scope access

For the application to access group information and handle login redirects correctly, you must configure two properties in the `sso` service dashboard. In order to access the `sso` dashboard, run the following command and go to the URL listed in `Dashboard` property:

```bash
$ cf service myOAuthService

Service instance: myOAuthService
Service: p-identity
bound apps: single-signon
Tags:
Plan: auth
Description: Single Sign-On as a Service
Documentation url: http://docs.pivotal.io/p-identity/index.html
Dashboard: https://p-identity.mypcf.example.com/dashboard/identity-zones/{ZONE_GUID}/instances/{INSTANCE_GUID}/...
```

On the dashboard, under `Apps`:

1. Select the `single-signon` app.
1. Click "Select Scopes" and add the `testgroup` scope
1. Add the URI `http://single-signon.<YOUR-CLOUDFOUNDRY-SYSTEM-DOMAIN>/signin-oidc` under "Auth Redirect URIs"
1. Click the "Save Config" button

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

On the apps menu, click on the `Log in` menu item and you should be redirected to the CloudFoundry login page. Enter `dave` and `Password1!`, or whatever name/password you used above,  and you should be authenticated and redirected back to the single-signon home page.

Two of the endpoints in the `HomeController` have Claims Authorization policys applied:

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