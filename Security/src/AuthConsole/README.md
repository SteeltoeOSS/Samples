# Steeltoe Application Security Worker/Console Client-side Authentication and Authorization

This application shows how to use the Steeltoe [security library](https://docs.steeltoe.io/api/v4/security/) for authentication and authorization with client certificates provided by Cloud Foundry or Steeltoe (when running locally).

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional: [Tanzu Platform for Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/concepts-overview.html)
   (optionally with [Windows support](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/toc-tasw-install-index.html))
   and [Cloud Foundry CLI](https://github.com/cloudfoundry/cli)

## Running locally

1. `dotnet run` both AuthApi and AuthConsole

## Running on Tanzu Platform for Cloud Foundry

1. Refer to the [AuthWeb README](../AuthWeb/README.md) for instructions on deploying AuthApi
   * If you are only interested in certificate authentication, skip everything related to Single Sign-On (SSO) and comment out or delete the `sampleSSOService` service reference from manifest(-windows).yml before `cf push`

1. Push AuthConsole to Cloud Foundry
   1. `cf target -o your-org -s your-space`
   1. `cd samples/Security/src/AuthConsole`
   1. `cf push`
     * When deploying to Windows, binaries must be built locally before push. Use the following commands instead:

     ```shell
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
     ```

> [!NOTE]
> The provided manifests will create apps named `auth-client-console-sample` and `auth-server-sample`
> and attempt to bind AuthApi to the SSO service `sampleSSOService`.

## What to expect

At this point the app is up and running. Since there is user interface for this worker, you can access the logs this command: `cf logs auth-client-console-sample`

---

See the Official [Steeltoe Security Documentation](https://docs.steeltoe.io/api/v4/security/) for more detailed information.
