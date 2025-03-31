# Network File Sharing Sample App

ASP.NET Core sample app showing how to use Steeltoe to manage credentialed connections with Windows file shares.

## General pre-requisites

1. Windows machine with the .NET 8 SDK installed
1. Pre-existing Windows file share or local adminstrator rights
1. Optional: [Tanzu Platform for Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/concepts-overview.html)
   with [Windows support](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/toc-tasw-install-index.html)

## Running locally

Before running the app, you need to create the fileshare or update `appsettings.Development.json` with the values for a pre-existing fileshare.

### Create a file share

> [!CAUTION]
> The script [create-user-and-share.ps1](../../Scripts/create-user-and-share.ps1) must be run as administrator.
> As with any script found on the internet, review the contents before running it.

1. Open a PowerShell window as an administrator
1. `cd` to the `scripts` directory`
1. Run [create-user-and-share.ps1](../../scripts/create-user-and-share.ps1), optionally using parameters to override the default values:
   * `-ShareName steeltoe_network_share` - the name of the share
   * `-SharePath c:\steeltoe_network_share` - the path to the share
   * `-UserName shareWriteUser` - the name of the user
   * `-Password thisIs1Pass!` - the password for the user

### Using an existing file share

1. Open the `appsettings.Development.json` file
1. Update the `location`, `username`, and `password` values with the appropriate values for your file share
1. Save the file

### Run the app

1. Open a terminal window
1. `cd` to the `FileSharesWeb` directory
1. Run the app with the following command:
    ```shell
    dotnet run
    ```

## Running on Tanzu Platform for Cloud Foundry

Before deploying the app, you must create an entry in CredHub to contain the credentials.

### Store credentials in CredHub

1. Run [cf-create-service.ps1](../../scripts/cf-create-service.ps1) to create a service instance in CredHub, using parameters to set the required values:
   * `-NetworkAddress \\\\<hostname>\\<sharename>` - Escaped UNC path of the fileshare. 
   * `-UserName <username>` - the username for accessing the fileshare
   * `-Password <password>` - the password for accessing the fileshare

### Deploy the app

1. This sample will only run on Windows, so binaries must be built locally before push. Use the following commands to publish and push the app:
    ```shell
    dotnet publish -r win-x64 --self-contained
    cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
    ```
1. Copy the value of `routes` in the output and open in your browser
