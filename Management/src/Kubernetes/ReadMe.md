# Kubernetes ASP.NET Core Sample Application

This ASP.NET Core sample uses the Steeltoe Kubernetes management library. All snippets below are executed from the directory `Samples\Management\src`.

## Pre-requisites

1. .NET Core SDK
1. Docker tooling
1. a Kubernetes cluster (a local cluster provided by Docker Desktop was used to build this sample)

## Build Image

Use docker to build an image with a version tag

```powershell
docker build -t kubernetesmanagement:v1 .\Kubernetes\
```

## Create Deployment

Create a Kubernetes release that references the tagged image

```powershell
kubectl create deployment kubernetesmanagement --image kubernetesmanagement:v1
```

## Expose Service

Expose the service so it can be accessed from a browser

```powershell
kubectl expose deployment kubernetes --port 8080 --target-port 80 --type=LoadBalancer
```

## View the service

Open <http://localhost:8080> in your browser. You should see a welcome message and some placeholders for data that will be retrieved from configuration as soon as it is present.

