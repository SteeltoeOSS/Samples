# CloudFoundry Single Signon Sample App (with TAS SSO Tile)

ASP.NET Core sample app illustrating how to use the Steeltoe [Cloud Foundry external security libraries](https://docs.steeltoe.io/api/v3/security/) for Authentication and Authorization against [Enterprise SSO for TAS for VMs](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service).

## Pre-requisites - Tanzu Application Service

1. Cloud Foundry instance
1. [CloudFoundry UAA Command Line Client](https://github.com/cloudfoundry/cf-uaac) installed
1. [SSO Tile](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service) installed

### Create a Service Plan

In order to use the SSO tile in an application, you must first [create a service plan](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service/1.14/sso/GUID-manage-service-plans.html).
The rest of this page assumes the `Auth Domain` for your SSO service plan is `auth`.

> Note: use the command `cf marketplace` to see your SSO plan once it has been created.

### Create a Service Instance

In order to bind SSO configuration information to our application, create a service instance: `cf create-service p-identity auth mySSOService`.

### Execute Script to Configure new User and Group

Next, locate the sso-setup script you'd like to execute (.cmd/.sh) from the scripts folder in this repository.
Update the variables to suit your environment - You will need the `Admin Client Secret` for your installation of CloudFoundry for this step.
If you are using Tanzu Application Service (TAS), you can obtain the secret from the `Ops Manager/Tanzu Application Service` credentials page under the `UAA` section.  Look for `Admin Client Credentials`.

### Push to Cloud Foundry

1. cf target -o myorg -s development
1. cd samples/Security/src/CloudFoundrySingleSignon
1. Push the app using the appropriate manifest:
    * `cf push`
    * `cf push -f manifest-windows.yml`

> Note: The provided manifest(s) will create an app named `single-signon` and attempt to bind it to the SSO service `mySSOService`.

### Configure SSO RedirectUri and Scope access

The RedirectUri and Scope access settings should be automatically configured via the settings in `manifest.yml`. If that configuration fails, the instructions below may be used to manually update the configuration.

For the application to access group information and handle login redirects correctly, configure two properties in the `sso` service dashboard. In order to access the `sso` dashboard, run the following command and go to the URL listed in `Dashboard` property:

```bash
$ cf service mySSOService

Service instance: mySSOService
Service: p-identity
bound apps: single-signon
Tags:
Plan: auth
Description: Single Sign-On as a Service
Documentation url: https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service
Dashboard: https://p-identity.mypcf.example.com/dashboard/identity-zones/{ZONE_GUID}/instances/{INSTANCE_GUID}/
...
```

On the dashboard, under `Apps`:

1. Select the `single-signon` app.
1. Click "Select Scopes" and add the `testgroup` scope
1. Add the URI `http://single-signon.<YOUR-SYSTEM-DOMAIN>/signin-cloudfoundry` under "Auth Redirect URIs"
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

The Home page of the application includes instructions for how to interact with services secured with JWT and Mutual TLS authentication.
