# Network File Sharing Sample App

ASP.NET Core sample app showing how to use Steeltoe to manage credentialed connections with Windows file shares.

## General pre-requisites

1. Windows machine with the .NET 10 SDK installed
1. Pre-existing Windows file share or local adminstrator rights
1. Optional: [Tanzu Platform for Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/concepts-overview.html)
   with [Windows support](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/toc-tasw-install-index.html)

## Running locally

Before running the app, you need to create the fileshare or update `appsettings.Development.json` with the values for a pre-existing fileshare.

### Create a file share

> [!CAUTION]
> The script [add-user-and-share.ps1](scripts/add-user-and-share.ps1) must be run as administrator, in Windows.
> As with any script found on the internet, review the contents before running it.

1. Open a PowerShell window as an administrator
1. `cd` to the `scripts` directory
1. Run [add-user-and-share.ps1](scripts/add-user-and-share.ps1), optionally using parameters to override the default values:
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
1. `cd` to the `src\FileSharesWeb` directory
1. Run the app with the following command:
    ```shell
    dotnet run --launch-profile https
    ```

Once the app is running, you should be able to [upload files](https://localhost:7032/files/upload) and [list files](https://localhost:7032/files/list) in the file share.
Multiple files can be uploaded at once using the form provided, but you should be aware that files are renamed when they are saved in order to prevent issues with improper characters.
You can also delete files by clicking the "Delete file" button in the same row as the file name on the list files page.

> [!TIP]
> The sample uses credentials different from those of your Windows user account. If you've opened the file share in Windows Explorer before running the sample, it fails because a file share can't be accessed by one user using multiple credentials. To recover, run `klist purge` to make Windows forget the connection from Windows Explorer.


### Removing the local user account and file share

> [!CAUTION]
> The script [remove-user-and-share.ps1](scripts/remove-user-and-share.ps1) must be run as administrator, in Windows.
> As with any script found on the internet, review the contents before running it.

When you are done working with the sample, you can remove the user account, the file share, and its target directory, with the following steps:

1. Open a PowerShell window as an administrator
1. `cd` to the `scripts` directory
1. Run [remove-user-and-share.ps1](scripts/remove-user-and-share.ps1), optionally using parameters to override the default values:
   * `-ShareName steeltoe_network_share` - the name of the share
   * `-SharePath c:\steeltoe_network_share` - the path to the share
   * `-UserName shareWriteUser` - the name of the user

## Running on Tanzu Platform for Cloud Foundry

Before deploying the app, you must create an entry in CredHub to contain the credentials.

### Store credentials in CredHub

> [!NOTE]
> The [cf-create-service.ps1](scripts/cf-create-service.ps1) script requires PowerShell 7 or later.

1. Run [cf-create-service.ps1](scripts/cf-create-service.ps1) to create a service instance in CredHub, using parameters to set the required values:
   * `-NetworkAddress \\<hostname>\<sharename>` - UNC path to the network share (required). For example: `\\localhost\steeltoe_network_share`
   * `-UserName <username>` - the username for accessing the fileshare, can include domain (e.g., `DOMAIN\username`) (required)
   * `-Password <password>` - the password for accessing the fileshare (required)
   * `-ServiceName credhub` - the name of the service
   * `-ServicePlan default` - the service plan
   * `-ServiceInstanceName sampleNetworkShare` - the name of the service instance

### Deploy the app

1. This sample will only run on Windows, so binaries must be built locally before push. Use the following commands to publish and push the app:
    ```shell
    dotnet publish -r win-x64 --self-contained
    cf push -f manifest-windows.yml -p bin/Release/net10.0/win-x64/publish
    ```
1. Copy the value of `routes` in the output and open in your browser

---

See the Official [Steeltoe Windows Network File Shares Documentation](https://docs.steeltoe.io/api/v4/fileshares/) for more detailed information.
