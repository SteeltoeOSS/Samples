# Kubernetes ASP.NET Core Sample Application

This ASP.NET Core sample uses the Steeltoe Kubernetes Management library, along with a companion Spring Boot Admin Server to access the actuator endpoints.
The application can run in or out of a Kubernetes cluster, and Steeltoe features will respond according to the environment in which they run.
In addition to the interface provided by Spring Boot Admin, you can interact with the endpoints directly with a base path of `<host:port>/actuator` (an HTTP GET request to this path will list all configured endpoints)

All snippets below are executed from the directory `Samples\Management\src\Kubernetes`.

## Pre-requisites

1. .NET Core SDK
1. Docker tooling
1. a Kubernetes cluster (a local cluster provided by Docker Desktop was used to build this sample)

## Build Image

Use docker to build an image with a version tag

```powershell
docker build -t steeltoe-management:v1 .
```

## Create Deployments

(Optionally) deploy a Spring Boot Admin server, exposed on port 9090 with the included yaml:

```powershell
kubectl apply -f .\SpringBootAdmin.yaml
```

Confirm the Spring Boot Admin server is up and running at <http://localhost:9090> before deploying the .NET application as Steeltoe's Spring Boot Admin Client will only attempt to register during application startup.

Create a Kubernetes release that references the tagged image, exposing the app on port 5000:

```powershell
kubectl apply -f .\SteeltoeDeployment.yaml
```

## View the service

Open <http://localhost:5000> in your browser. The application itself is trivial, but you should see a welcome message for basic confirmation the app is running. Actuators should be accessible at <http://localhost:5000/actuator> or via the admin server at <http://localhost:9090>.
