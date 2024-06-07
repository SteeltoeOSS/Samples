# Steeltoe Application Security Server-side Authentication and Authorization

This application shows how to use the Steeltoe [security libraries](https://docs.steeltoe.io/api/v3/security/) for authentication and authorization with JWT Bearer tokens against [Single Sign-On for VMware Tanzu Application Service](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service) and using client certificates provided by Cloud Foundry or Steeltoe (when running locally).

This sample illustrates how you can secure your web api endpoints using JWT Bearer tokens and client certificates.

> Note: This application is intended to be used in conjunction with the [AuthClient](../AuthClient) sample app. You should get that sample up and running first and follow these instructions after that.

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional: [VMware Tanzu Platform for Cloud Foundry](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/index.html)
   (optionally with [Windows support](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/concepts-overview.html))
   with [Single Sign-On for VMware Tanzu Application Service](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service)
   and [Cloud Foundry CLI](https://docs.cloudfoundry.org/cf-cli/install-go-cli.html)

## Running locally

1. Start a UAA Server [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md) if it isn't already running
1. `dotnet run`

## Running on Tanzu Platform for Cloud Foundry

1. If you haven't already, follow the steps to get [AuthClient](../AuthClient) up and running
1. Push to Cloud Foundry
   1. `cf target -o myorg -s development`
   1. `cd samples/Security/src/ClientAuth`
   1. `cf push`
    * When deploying to Windows, binaries must be built locally before push. Use the following commands instead:
     ```
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
     ```

> Note: The provided manifests will create an app named `steeltoe-samples-authserver` and attempt to bind it to the SSO service `mySSOService`.

> Note: `mySSOService` is created when you follow the instructions for [AuthClient](../AuthClient/README.md).

## What to expect

At this point the app is up and running. You can access it at <https://localhost:7184> or <https://steeltoe-samples-authserver.`YOUR-CLOUDFOUNDRY-APP-DOMAIN`/>.

> Note: To see the logs on Cloud Foundry as the app runs, execute this command: `cf logs steeltoe-samples-authserver`

---
### See the Official [Steeltoe Security Documentation](https://docs.steeltoe.io/api/v3/security/) for more detailed information.
