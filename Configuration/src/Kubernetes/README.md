# Kubernetes Configuration ASP.NET Core Sample Application

This ASP.NET Core sample uses the Steeltoe configuration providers for Kubernetes ConfigMaps and Secrets. All snippets below are executed from the directory `Samples\Configuration\src`.

## Pre-requisites

1. .NET Core SDK
1. Docker tooling
1. a Kubernetes cluster (a local cluster provided by Docker Desktop was used to build this sample)

## Service Account Setup

In order to use the Kubernetes configuration providers, your app needs to execute under a service account that has permission to interact with the Kubernetes API.
The file `service-account.yaml` has been provided for a straightforward getting-started experience with the default account.

```powershell
 kubectl apply -f .\Kubernetes\service-account.yaml
 ```

## Build Image

Use docker to build an image with a version tag

```powershell
docker build -t kubernetes:v1 .\Kubernetes\
```

## Create Deployment

Create a Kubernetes release that references the tagged image

```powershell
kubectl create deployment kubernetes --image kubernetes:v1
```

## Expose Service

Expose the service so it can be accessed from a browser

```powershell
kubectl expose deployment kubernetes --port 8080 --target-port 80 --type=LoadBalancer
```

## View the service

Open <http://localhost:8080> in your browser. You should see a welcome message and some placeholders for data that will be retrieved from configuration as soon as it is present.

## Add ConfigMaps

Several config maps have been provided in the file `configmaps.yaml`, use `kubectl` to deploy them:

```powershell
kubectl apply -f .\Kubernetes\configmaps.yaml
```

>This sample uses the default settings of refresh by polling. ConfigMap additions and updates will be visible within 15 seconds.

## Add Secrets

Several secrets have been provided in the file `secrets.yaml`, use `kubectl` to deploy them:

```powershell
kubectl apply -f .\Kubernetes\secrets.yaml
```

>This sample uses the default settings of refresh by polling. Secret additions and updates will be visible within 15 seconds.

## Remarks

Steeltoe uses some basic conventions for finding ConfigMaps and Secrets that follow the pattern set by `appsettings.json`. By default, these configuration providers will find ConfigMap or Secret resources named `<appName>` and `<appName>.<EnvironmentName>`, where `<appName>` is defined by `spring:application:name`, `spring:cloud:kubernetes:name` and falling back on the name of the assembly as a last resort. See the documentation for further configuration options.
