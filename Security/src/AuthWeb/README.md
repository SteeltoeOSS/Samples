# Steeltoe Application Security Client-side Authentication and Authorization

This application shows how to use the Steeltoe [security libraries](https://docs.steeltoe.io/api/v3/security/) for authentication and authorization with OpenID Connect against [Single Sign-On for VMware Tanzu Application Service](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service) and using client certificates provided by Cloud Foundry or Steeltoe (when running locally).

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional: [VMware Tanzu Platform for Cloud Foundry](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/index.html)
   (optionally with [Windows support](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/concepts-overview.html))
   with [Single Sign-On for VMware Tanzu Application Service](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service)
   and [Cloud Foundry CLI](https://docs.cloudfoundry.org/cf-cli/install-go-cli.html)

## Running locally

1. Start a UAA Server [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)
1. `dotnet run` both AuthWeb and AuthApi
1. Please note that some of the links in the menu won't work until you also start the [AuthApi](../AuthApi/README.md) application

## Running on Tanzu Platform for Cloud Foundry

1. Install [Single Sign-On for VMware Tanzu Application Service](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service)
1. Deploy [UAA with the Steeltoe Samples configuration](https://github.com/SteeltoeOSS/Dockerfiles/tree/main/uaa-server#customizing-for-your-environment)
1. Create a [service plan](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service/1.14/sso/GUID-manage-service-plans.html)
1. Configure federated authentication on the service plan
   1. Add the UAA server from step 2 to the service as an [OIDC Provider](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service/1.14/sso/GUID-configure-external-id.html#config-ext-oidc)
      * Name the identity provider `steeltoe-uaa` (or update `SSO_IDENTITY_PROVIDERS` in manifest.yml accordingly)
      * Credentials for connecting to the UAA server can be found or customized before deployment in [uaa.yml](https://github.com/SteeltoeOSS/Dockerfiles/blob/main/uaa-server/uaa.yml#L124)
   1. Save changes, but keep this page open
1. Create a service instance:
   * `cf create-service p-identity <your service plan name> sampleSSOService`
1. Push AuthApi to Cloud Foundry
   1. `cf target -o your-org -s your-space`
   1. `cd samples/Security/src/AuthApi`
   1. `cf push`
     * When deploying to Windows, binaries must be built locally before push. Use the following commands instead:
     ```
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
     ```
1. Return to the service plan setup page and add an External Group Mapping with these values:
      * OIDC Groups Claim Name = scope
      * External Group Name = openid
      * Permissions = sampleapi.read (If this option isn't available, ensure AuthApi has been deployed)
1. Push AuthWeb to Cloud Foundry
   1. `cf target -o your-org -s your-space`
   1. `cd samples/Security/src/AuthWeb`
   1. `cf push`
     * When deploying to Windows, binaries must be built locally before push. Use the following commands instead:
     ```
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
     ```

> [!NOTE]
> The provided manifests will create apps named `auth-client-sample` and `auth-server-sample`
> and attempt to bind both to the SSO service `sampleSSOService`.

### RedirectUri and Scope access

The RedirectUri and Scope access settings should be automatically configured via the settings in `manifest.yml`.

If you want to access the `sso` dashboard, run the following command and go to the URL listed in `dashboard url` property:

```bash
$ cf service sampleSSOService

name:            sampleSSOService
guid:            ea8b8ac0-ce85-4726-8b39-d1b2eb55b45b
type:            managed
broker:          identity-service-broker
offering:        p-identity
plan:            steeltoe
tags:
offering tags:
description:     Provides identity capabilities via UAA as a Service
documentation:   https://docs.pivotal.io/p-identity/index.html
dashboard url:   https://p-identity.sys.cf-app.com/developer/identity-zones/15aaabfa-0697-4ad7-96a8-ed81c0a286a7/instances/ea8b8ac0-ce85-4726-8b39-d1b2eb55b45b/
...
```

## What to expect

At this point the app is up and running.  You can access it at <https://localhost:7072> or <https://auth-client-sample.`YOUR-CLOUDFOUNDRY-APP-DOMAIN`/>.

> [!NOTE]
> To see the logs on Cloud Foundry as the app runs, execute this command: `cf logs auth-client-sample`

From the website's menu, click on the `Log in` menu item and you should be redirected to the UAA server's login page. Enter `testuser` and `password`, and you should be authenticated and redirected back to the auth client home page.

The menu of the application includes links for testing the permissions of the user in the current application and interact with another service that has been secured with JWT and client certificates.

* The JWT menu item uses the current user's token to communicate with the backend service.
* The "SameSpace" and "SameOrg" menu items interact with the backend service using an identity certificate that belongs to the application.
   * Locally, certificates for both the client and server are created by Steeltoe.
   * On Cloud Foundry, certificates are provisioned by the platform, with OrgId and SpaceId populated based on where the applications are deployed.
* While logged in, view information about the testuser account by clicking on "Hello testuser!" next to the "Log out" link.
* If needed, sign out of the UAA server using the dropdown menu in the top right corner at <http://localhost:8080>(locally) or use the command `cf app steeltoe-uaa` to get the address of the server deployed to Cloud Foundry.

---

See the Official [Steeltoe Security Documentation](https://docs.steeltoe.io/api/v3/security/) for more detailed information.
