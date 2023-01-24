# PostgreSQL Connector Sample App - NpgsqlConnection

ASP.NET Core sample app illustrating how to use [Steeltoe PostgreSQL Connector](https://docs.steeltoe.io/api/v3/connectors/postgresql.html) for connecting to a PostgreSQL service on CloudFoundry.
This sample illustrates using a `NpgsqlConnection` to issue commands to the bound database. There is also an additional sample that illustrates how to use Entity Framework Core.

## General prerequisites

1. Installed .NET Core SDK

## Running locally

1. Started PostgreSQL [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)

## Running on CloudFoundry

1. Installed CloudFoundry (optionally with Windows support)
1. Installed [VMware Tanzu Cloud Service Broker](https://docs.vmware.com/en/Cloud-Service-Broker-for-VMware-Tanzu/index.html)

## Create PostgreSQL Service Instance on CloudFoundry

You must first create an instance of the PostgreSQL service in an org/space.

1. `cf target -o your-org -s your-space`
1. `cf create-service csb-azure-postgresql small myPostgreSqlService` or `cf create-service csb-google-postgres default myPostgreSqlService`

## Publish App & Push to CloudFoundry

1. `cf target -o your-org -s your-space`
1. `cd samples/Connectors/src/PostgreSql`
1. Push the app
   - When using Windows containers:
     - Publish app to a local directory, specifying the runtime:
       * `dotnet restore --configfile nuget.config`
       * `dotnet publish -r win-x64 --self-contained`
     - Push the app using the appropriate manifest:
       * `cf push -f manifest-windows.yml -p bin/Debug/net6.0/win-x64/publish`
   - Otherwise:
     - Push the app using the appropriate manifest:
       * `cf push -f manifest.yml`

> Note: The provided manifest(s) will create an app named `postgres-connector` and attempt to bind the app to PostgreSQL service `myPostgreSqlService`.

## What to expect - CloudFoundry

To see the logs as you startup and use the app: `cf logs postgresql-connector`

On a Windows cell, you should see something like this during startup:

```text
2016-08-05T07:23:02.15-0600 [CELL/0]     OUT Creating container
2016-08-05T07:23:03.81-0600 [CELL/0]     OUT Successfully created container
2016-08-05T07:23:09.07-0600 [APP/0]      OUT Running .\PostgreSql
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Hosting environment: development
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Content root path: C:\containerizer\75E10B9301D2D9B4A8\user\app
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Now listening on: http://*:51217
```

This sample will be available at <http://postgresql-connector.[your-cf-apps-domain]/>.

Upon startup, the app inserts a couple of rows into the bound PostgreSQL database. They are displayed on the home page.

---

> Note: The provided manifest(s) will create an app named `postgres-connector` and attempt to bind the app to PostgreSQL service `myPostgreSqlService`.

## Running on Tanzu Application Platform (TAP)
1. Set up Postgres services for [consumption by developers] (https://docs.vmware.com/en/VMware-Tanzu-Application-Platform/1.3/tap/GUID-getting-started-set-up-services.html)

## Create PostgreSQL Service Instance/Binding on TAP
1. Created a [Postgres Service Instance](https://docs.vmware.com/en/VMware-SQL-with-Postgres-for-Kubernetes/2.0/vmware-postgres-k8s/GUID-create-delete-postgres.html)
1. Create a [Postgres Service Binding/Claim](https://docs.vmware.com/en/VMware-SQL-with-Postgres-for-Kubernetes/2.0/vmware-postgres-k8s/GUID-creating-service-bindings.html)


## Publish App & Push to TAP
1. `cd samples/Connectors/src/PostgreSql`
1. Bind the TAP Workload to the Service Binding/Claim by modifying the provided `workload.yml` `serviceClaims` section with the claim name created above.
1. Push the app to TAP:
  * `tanzu app workload apply <app_name> --local-path .\bin\Debug\net6.0 --source-image <registry-reference> -f workload.yml`


### See the Official [Steeltoe Service Connectors Documentation](https://docs.steeltoe.io/api/v3/connectors/) for a more in-depth walkthrough of the samples and more detailed information.
