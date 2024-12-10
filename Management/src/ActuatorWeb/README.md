# Steeltoe Management Sample - Actuators, Administrative Tasks, Metrics and Tracing

ActuatorWeb and ActuatorAPI form an ASP.NET Core-powered sample application that demonstrates how to use several
Steeltoe libraries on their own and with additional tools such
as [Tanzu Apps Manager](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/console-index.html)
and [Spring Boot Admin](https://docs.spring-boot-admin.com/) used for managing and monitoring applications.

This application also illustrates how to have application metrics captured and exported to
the [Metrics Registrar](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/metric-registrar-index.html)
service so that application metrics can be viewed in any tool that is able to consume those metrics from
the [Cloud Foundry Loggregator](https://github.com/cloudfoundry/loggregator-release).
Several tools exist that can do this,
including [App Metrics for VMware Tanzu](https://docs.vmware.com/en/App-Metrics-for-VMware-Tanzu/2.2/app-metrics/GUID-index.html).

> [!NOTE]  
> While this application is intended to be used with [ActuatorApi](../ActuatorApi/), it _can_ be used on its own if you
> are looking for a simpler actuator or metrics example.

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional:
    * [VMware Tanzu Platform for Cloud Foundry](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/index.html)
    * [Cloud Foundry CLI](https://docs.cloudfoundry.org/cf-cli/install-go-cli.html)
    * [Metrics Registrar for VMware Tanzu Application Service](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/metric-registrar-index.html)
    * [App Metrics for VMware Tanzu](https://docs.vmware.com/en/App-Metrics-for-VMware-Tanzu/2.2/app-metrics/GUID-index.html)
    * [VMware MySQL for Tanzu Application Service](https://docs.vmware.com/en/VMware-SQL-with-MySQL-for-Tanzu-Application-Service/index.html)
    or [VMware Tanzu Cloud Service Broker](https://docs.vmware.com/en/Cloud-Service-Broker-for-VMware-Tanzu/index.html)

## Running locally

This application has a number of moving pieces, some of which can be used in multiple ways. In order to experience all
of the functionality in a local environment, you will need to meet additional pre-requisites:

* Installed container orchestrator (such as [Docker Desktop](https://www.docker.com/products/docker-desktop/)
  or [Podman Desktop](https://podman-desktop.io/))
* Optional: an IDE (such as Visual Studio), for performing the requests defined
  in [ActuatorWeb.http](./ActuatorWeb.http) and [ActuatorApi.http](../ActuatorApi/ActuatorApi.http).

> [!NOTE]  
> Use the [docker-compose file](../docker-compose.yaml) to run all of the backing services used by these samples.

### Spring Boot Admin

Because Steeltoe Management Endpoints are compatible with Spring Boot Actuator, they can also be used
with [Spring Boot Admin](https://docs.spring-boot-admin.com/). Refer
to [Common Tasks](../../../CommonTasks.md#spring-boot-admin) to start Spring Boot Admin.

This application is configured to register itself with Spring Boot Admin running at <http://localhost:9090>. The
configuration defined in [appsettings.Development.json](./appsettings.Development.json) (under
`Spring:Boot:Admin:Client`) instructs Spring Boot Admin to reach this instance's actuators on port 9990 (defined
in [appsettings.json](./appsettings.json)), using basic authentication and a hostname (`host.docker.internal`) that is
routeable from within the container network. Change the configuration if you are using Podman or don't want actuators on
a dedicated port. All of the above is also true for ActuatorApi, except that application has actuators configured on
port 9991.

### Zipkin Server

> [!NOTE]  
> Previous versions of Steeltoe included distributed tracing functionality. That functionality has moved to OpenTelemetry.
> We expect many Steeltoe users still want distributed tracing, so this sample includes a basic OpenTelemetry setup.
> Visit [OpenTelemetry](https://opentelemetry.io/docs/languages/net/) to learn more. 

These applications use [OpenTelemetry](./OpenTelemetryExtensions.cs) to instrument all HTTP interactions with
trace information and export those traces to Zipkin so that individual requests across the system can be analyzed. Refer
to [Common Tasks](../../../CommonTasks.md#zipkin) to start Zipkin.

### Starting the application

You should now be able to run this sample, either from your IDE or with this command:

```shell
dotnet run
```

> [!NOTE]  
> If you are only looking for an actuator demo, you _may_ proceed from here without starting ActuatorApi, but for full
> functionality, review the [ActuatorApi README](../ActuatorApi/README.md) and get that app running as well.

### Interacting with the application

As ActuatorWeb is a Razor Pages app, you can use a browser to [access the web UI](http://localhost:5126) while the app
is running. To request a weather forecast (and generate HTTP request traces) from ActuatorApi, click "Weather Forecast"
in the site menu.

### Interacting with Management Endpoints

Each app instance registers with Spring Boot Admin and can be viewed from the web interface at <http://localhost:9090>.

[ActuatorWeb.http](./ActuatorWeb.http) and [ActuatorApi.http](../ActuatorApi/ActuatorApi.http) are also provided for
experimentation with the exposed actuators. Variables for the .http files are stored
in [http-client.env.json](./http-client.env.json). In both Visual Studio and Rider, when you open the .http file there
is a dropdown box that can be used to switch between localhost variations (for example: actuators listening on an
alternate port, basic authentication on/off) or environments. The dropdown is probably at the top of the window and may
initially look like `env: <none>` or `Run With: No Environment`, although those interfaces are subject to change over
time. You will need to either make an environment selection or define variables like `HostAddress` directly in the .http
file in order for requests to work (review the .json file mentioned previously for variable examples). Please note that
the "dhaka" environment is not expected to be available for use by those not on the Steeltoe team.

#### Custom Management Endpoints

This sample includes a custom actuator that can be used to return the time on the server where the app is running.
Review the contents of [./CustomActuators](./CustomActuators/) to see how this integration is accomplished.

### Reviewing HTTP Traces

1. Open the [Zipkin web UI on your machine](http://localhost:9411)
1. Click the plus button near the top of the page
1. Select `serviceName`, then `steeltoe.samples.actuatorweb`
1. Click `RUN QUERY`
1. Explore trace data

> Only requests for `steeltoe.samples.actuatorweb: get weather` will actually be distributed traces. You may want to
> request the weather from ActuatorWeb a few times to generate interesting data.

## Running on Tanzu Platform for Cloud Foundry

1. Run the `cf push` command to deploy from source (you can monitor logs with `cf logs actuator-web-management-sample`)
    * When deploying to Windows, binaries must be built locally before push. Use the following commands instead:

      ```shell
      dotnet publish -r win-x64 --self-contained
      cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
      ```

1. Copy the value of `routes` in the output and open in your browser
1. Refer to [ActuatorApi README](../ActuatorApi/README.md#running-on-tanzu-platform-for-cloud-foundry) for additional
   instructions.

### What to expect

Once the app is up and running, then you can access the management endpoints exposed by Steeltoe
using [Apps Manager](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/manage-apps.html).

Steeltoe exposes Spring Boot Actuator compatible Endpoints which can be accessed via the Tanzu Apps Manager. By using
the Apps Manager, you can view the App's Health, Build Information (for example: Git info, etc), as well as view or
change the application's logging levels.

Check out the Apps
Manager, [Using Spring Boot Actuators](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/using-actuators.html)
for more information.

### Install the Metrics Registrar CLI Plugin

1. cf install-plugin -r CF-Community "metric-registrar"

### View Application Metrics in App Metrics

If you wish to collect and view applications metrics
in [App Metrics for VMware Tanzu](https://docs.vmware.com/en/App-Metrics-for-VMware-Tanzu/index.html), you must first
configure [Metrics Registrar](https://docs.pivotal.io/platform/application-service/2-9/metric-registrar/index.html) in
the TAS for VMs tile. There is no separate product tile (unlike the metrics forwarder). Once thats complete custom
metrics will be collected and automatically exported to the Metrics Forwarder service.

1. cf target -o myOrg -s development
2. cf register-metrics-endpoint actuator-web-management-sample actuator/metrics
3. cf restart actuator-api-management-sample

If you wish to collect and view applications metrics
in [App Metrics for VMware Tanzu](https://docs.vmware.com/en/App-Metrics-for-VMware-Tanzu/index.html), you must first
configure [Metrics Registrar](https://docs.pivotal.io/platform/application-service/2-9/metric-registrar/index.html) in
the TAS for VMs tile. There is no separate product tile (unlike the metrics forwarder). Once that's complete, custom
metrics will be collected and automatically exported to the Metrics Forwarder service.

---

See the Official [Steeltoe Management Documentation](https://docs.steeltoe.io/api/v3/management/) for more information.
