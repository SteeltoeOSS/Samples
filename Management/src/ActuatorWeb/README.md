# Steeltoe Management Sample - Actuators, Administrative Tasks, Metrics and Tracing

ActuatorWeb and ActuatorApi form an ASP.NET Core-powered sample application that demonstrates how to use several Steeltoe libraries on their own and with additional tools such as
[Tanzu Apps Manager](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/console-index.html) and
[Spring Boot Admin](https://docs.spring-boot-admin.com/) used for managing and monitoring applications.

These samples also illustrate how to have application metrics captured and exported to the [Metrics Registrar](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/metric-registrar-index.html) service so that application metrics can be viewed in any tool that is able to consume those metrics from the [Cloud Foundry Loggregator](https://github.com/cloudfoundry/loggregator-release).
Several tools exist that can do this, including [App Metrics for VMware Tanzu](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform-services/app-metrics-for-tanzu/2-2/app-metrics/index.html).

> [!NOTE]  
> While this application is intended to be used with [ActuatorApi](../ActuatorApi/), it _can_ be used on its own if you
> are looking for a simpler actuator or metrics example.

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional:
    * [VMware Tanzu Platform for Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/concepts-overview.html)
    * [Cloud Foundry CLI](https://docs.cloudfoundry.org/cf-cli/install-go-cli.html)
    * [Metrics Registrar for VMware Tanzu Application Service](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/metric-registrar-index.html)
    * [App Metrics for VMware Tanzu](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform-services/app-metrics-for-tanzu/2-2/app-metrics/index.html)
    * [VMware MySQL for Tanzu Application Service](https://techdocs.broadcom.com/us/en/vmware-tanzu/data-solutions/tanzu-for-mysql-on-cloud-foundry/3-3/mysql-for-tpcf/about_mysql_vms.html)
    or a [VMware Tanzu Cloud Service Broker](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform-services.html)

## Running locally

This application has a number of moving pieces, some of which can be used in multiple ways.
In order to experience all of the functionality in a local environment, you will need to meet additional pre-requisites:

* Installed container orchestrator (such as [Docker Desktop](https://www.docker.com/products/docker-desktop/) or [Podman Desktop](https://podman-desktop.io/))
* Optional: an IDE (such as Visual Studio), for performing the requests defined in [ActuatorWeb.http](./ActuatorWeb.http) and [ActuatorApi.http](../ActuatorApi/ActuatorApi.http).

> [!NOTE]  
> Use the [docker-compose file](../docker-compose.yaml) to run all of the backing services used by these samples.
>
> ```shell
> cd Management\src
> docker-compose up --pull always --detach --wait
> ```

### Spring Boot Admin

Because Steeltoe Management Endpoints are compatible with Spring Boot Actuator, they can also be used with [Spring Boot Admin](https://docs.spring-boot-admin.com/).
Refer to [Common Tasks](../../../CommonTasks.md#spring-boot-admin) to start Spring Boot Admin.

This application is configured to register itself with Spring Boot Admin running at <http://localhost:9099>. The configuration defined in [appsettings.Development.json](./appsettings.Development.json) (under `Spring:Boot:Admin:Client`) instructs Spring Boot Admin to reach this instance's actuators on port 8090 (defined in [appsettings.json](./appsettings.json)), using basic authentication and a hostname (`host.docker.internal`) that is routable from within the container network.
Change the configuration if you are using Podman (see the section [Using Podman](#using-podman)) or don't want actuators on a dedicated port.
All of the above is also true for ActuatorApi, except that application has actuators configured on port 8091.

> [!NOTE]  
> Please be aware that changes to the actuator port (or scheme) will also affect Prometheus. Be sure to update [prometheus.yml](../prometheus/prometheus.yml) accordingly.

### Zipkin Server

> [!NOTE]  
> Previous versions of Steeltoe included distributed tracing functionality. That functionality has moved to OpenTelemetry.
> We expect many Steeltoe users still use distributed tracing, so this sample includes the same OpenTelemetry configuration as prior versions of Steeltoe.
> Visit [OpenTelemetry](https://opentelemetry.io/docs/languages/net/) to learn more.

These applications use [OpenTelemetry extension methods](./OpenTelemetryExtensions.cs) to instrument all HTTP interactions with trace information and export those traces to Zipkin so that individual requests across the system can be analyzed.
Refer to [Common Tasks](../../../CommonTasks.md#zipkin) to start Zipkin.

### Starting the application

You should now be able to run this sample, either from your IDE or with this command:

```shell
dotnet run
```

> [!NOTE]
> If you are only looking for an actuator demo, you _may_ proceed from here without starting ActuatorApi, but for full functionality, review the [ActuatorApi README](../ActuatorApi/README.md) and get that app running as well.

### Interacting with the application

As ActuatorWeb is a Razor Pages app, you can use a browser to [access the web UI](http://localhost:5126) while the app is running.
To request a weather forecast (and generate HTTP request traces) from ActuatorApi, click "Weather Forecast" in the site menu.

> [!NOTE]
> The Weather Forecast API endpoint is delayed randomly up to three seconds to generate interesting trace data.

### Interacting with Management Endpoints

Each app instance registers with Spring Boot Admin and can be viewed from the web interface at <http://localhost:9099>.

[ActuatorWeb.http](./ActuatorWeb.http) and [ActuatorApi.http](../ActuatorApi/ActuatorApi.http) are also provided for experimentation with the exposed actuators.
Variables for the .http files are stored in [http-client.env.json](./http-client.env.json).
In both Visual Studio and Rider, when you open the .http file there is a dropdown box that can be used to select the environment you're working in (for example: running locally with actuators listening on an alternate port with basic authentication on).
The dropdown is probably at the top of the window and may initially look like `env: <none>` or `Run With: No Environment`, although those interfaces are subject to change over time.
You will need to either make an environment selection or define variables like `HostAddress` directly in the .http file in order for requests to work (review the .json file mentioned previously for variable examples).
Please note that the "dhaka" environment is not expected to be available for use by those not on the Steeltoe team.

#### Regarding HTTPS and Basic Authentication

In order to work around certificate trust issues with requests to the application from containerized servers (such as Spring Boot Admin), these applications are configured to allow _HTTP_ requests to the actuator endpoints. This has the unfortunate side effect of requiring HTTPS redirection to be turned off, lest requests to the actuators' dedicated HTTP port be forwarded to the application's HTTPS port.

In order to provide a simple example of applying custom authorization policies to actuator endpoints, these applications use [idunno.Authentication.Basic](https://github.com/blowdart/idunno.Authentication/tree/dev/src/idunno.Authentication.Basic).
Do NOT consider this a recommendation or a good example to follow, it is merely a general demonstration of how to apply [ASP.NET Core Authorization](https://learn.microsoft.com/aspnet/core/security/authorization/introduction).
Use a more robust authorization mechanism with your applications.

> [!CAUTION]
> Outside of a private network, this combination is effectively unsecured and is not recommended.

#### Custom Management Endpoints

This sample includes a custom actuator that can be used to return the time on the server where the app is running.
Review the contents of [./CustomActuators](./CustomActuators/) to see how this integration is accomplished.

### Reviewing HTTP Traces

1. Open the [Zipkin web UI on your machine](http://localhost:9411)
1. Click the plus button near the top of the page
1. Select `serviceName`, then `actuatorweb`
1. Click `RUN QUERY`
1. Explore trace data

> [!TIP]
> Only requests for `actuatorweb: get weather` will actually be distributed traces. You may want to request the weather from ActuatorWeb a few times to generate interesting data.

### Viewing Metric Dashboards

If you have used the docker compose file to start all of the services, Prometheus and Grafana should already be running (and regularly capturing your app metrics). Use [this link](http://localhost:3000) to access Grafana, where you should be greeted by a dashboard showing metrics from the sample app. You can also access the [Prometheus web interface](http://localhost:9090) if you'd like, but instructions for using Prometheus go beyond the scope of this document.

> [!TIP]
> To quickly confirm that Prometheus is able to scrape the application endpoints, check [the target health page](http://localhost:9090/targets)

#### Prometheus

To launch Prometheus with the configuration for this demo, but without using the docker compose file:

1. Open a shell, `cd` to `\Management\src`
1. `docker run  --rm -it --pull=always -p 9090:9090 -v $PWD/prometheus:/etc/prometheus prom/prometheus`

#### Grafana

To launch Grafana with the configuration for this demo, but without using the docker compose file:

1. Open a shell, `cd` to `\Management\src`
1. `docker run  --rm -it --pull=always -p 3000:3000 -v $PWD/grafana/config:/etc/grafana -v $PWD/grafana/dashboards:/var/lib/grafana/dashboards grafana/grafana`

For more information about using Grafana dashboards, see the [Grafana documentation](https://grafana.com/docs/grafana/latest/dashboards/use-dashboards/).

## Using Podman

Because Podman is generally compatible with Docker, the only changes necessary to run this sample with Podman is to change the hostname in places where `host.docker.internal` is used.

When running with Podman, update these files to use `host.containers.internal`:

* [ActuatorWeb appsettings.Development.json](./appsettings.Development.json)
* [ActuatorApi appsettings.Development.json](../ActuatorApi/appsettings.Development.json)
* [prometheus.yml](../prometheus/prometheus.yml)
* [Grafana data source definition](../grafana/config/provisioning/datasources/default.yaml)

## Running on Tanzu Platform for Cloud Foundry

1. Run the `cf push` command to deploy from source (you can monitor logs with `cf logs actuator-web-management-sample`)
    * When deploying to Windows (or to see `git.properties` in the Info actuator response), binaries must be built locally before push. Use the following commands instead:

      ```shell
      dotnet publish -r win-x64 --self-contained
      cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
      ```

> [!NOTE]
> These applications use the GitInfo NuGet package to write a `git.properties` if the .git folder is found.
> When the staging process runs on Cloud Foundry, that information is not available.
> If you want to see git properties returned when the application is running on Cloud Foundry, publish the application before pushing.

2. Copy the value of `routes` in the output and open in your browser
3. Refer to [ActuatorApi README](../ActuatorApi/README.md#running-on-tanzu-platform-for-cloud-foundry) for additional instructions.

### What to expect

Once the app is up and running, then you can access the management endpoints exposed by Steeltoe using Apps Manager.

Steeltoe exposes Spring Boot Actuator compatible endpoints which can be accessed via the Tanzu Apps Manager.
By using the Apps Manager, you can view the app's health, build information (for example: Git info, etc), as well as view or change the application's minimum logging levels.

Check out the Apps Manager, [Using Spring Boot Actuators](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/using-actuators.html) for more information.

Apps Manager should be accessible at <https://apps.sys.your.domain>, contact your platform administrator for assistance as needed.

### View Application Metrics in App Metrics

App Metrics should be accessible at <https://metrics.sys.your.domain>, contact your platform administrator for assistance as needed. Container metrics should automatically be available.
If you wish to collect and view application metrics, the [Metrics Registrar](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/metric-registrar-index.html#configure) must be configured, the metric-registrar cli plugin should be installed, and your Prometheus endpoint must be registered. Once that's complete, custom metrics will be collected and automatically exported to App Metrics.

> [!CAUTION]
> The command `register-metrics-endpoint` described below does not work in Windows, but does work in WSL. See [here](https://github.com/pivotal-cf/metric-registrar-cli/issues/4) for more information.

1. `cf install-plugin -r CF-Community "metric-registrar"`
1. `cf target -o myOrg -s development`
1. `cf register-metrics-endpoint actuator-web-management-sample /actuator/prometheus --internal-port 8090`
1. `cf register-metrics-endpoint actuator-api-management-sample /actuator/prometheus --internal-port 8091`
1. [Add your own metric charts](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform-services/app-metrics-for-tanzu/2-2/app-metrics/using.html#custom-metrics)
   1. Use an included .http file to send a request to the Prometheus to see what metrics are available
   1. Try the query `sum(process_runtime_dotnet_gc_objects_size_bytes{source_id="$sourceId"})` to see how much memory is in use by objects in the GC heap that haven't been collected yet

> [!NOTE]
> Prometheus scraping on Cloud Foundry cannot be configured with authentication. As such, we recommend using a dedicated port that is not internet-routable.
> This is the reason these applications are configured to map actuators (and the Prometheus exporter) to dedicated ports that are not exposed to the internet.

---

See the Official [Steeltoe Management Documentation](https://docs.steeltoe.io/api/v3/management/) for more information.
