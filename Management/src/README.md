# Management Sample Applications

This repo tree contains sample apps illustrating how to use the Steeltoe Management Endpoints and Management Tasks for managing and monitoring ASP.NET Core applications. Steeltoe Management Endpoints can be used to expose services for checking health, adjusting logging, etc. of your applications. The endpoints also seamlessly integrate with [Tanzu Apps Manager](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/console-index.html), providing more information on how your application is behaving.

## ASP.NET Core Samples

* src/ActuatorWeb - use Management Endpoints and distributed tracing. Some functionality depends on ActuatorApi.
* src/ActuatorApi - use Management Endpoints, Management Tasks and distributed tracing.

## Building & Running

See the Readme for instructions on building and running each app.

---

See the Official [Steeltoe Management Documentation](https://docs.steeltoe.io/api/v3/management/) for more information.
