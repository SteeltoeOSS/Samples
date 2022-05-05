# Fortune-Teller-Service - ASP.NET Core Micro-service

This sample application also demonstrates how to add metrics and traces using [Tanzu Observability by Wavefront](https://docs.wavefront.com/wavefront_introduction.html) .
When configured, your .NET application will send metrics and traces to a wavefront instance either directly or via a proxy. 

## Pre-requisites 

### Tanzu Observability Account Information

This sample assumes that there is a wavefront account with access to an API token. 

1. If you don't already have this, you can Sign up for a free trial account https://tanzu.vmware.com/observability-trial
1. Configure your account https://docs.wavefront.com/users_account_managing.html
1. Note the reporting url and the Api Token 

### Adding Wavefront Metrics
Add WavefrontMetrics using any of the available extension methods with the name `AddWavefrontMetrics`.  Here is an example using the WebHostBuilder Extension method. 
You can also use the Generic Host or Service Collection extension methods. 

```csharp
  WebHost.CreateDefaultBuilder(args)
                  ...
                   .AddWavefrontMetrics()
```
In addition you will need to add some configuration described below in the Wavefront Configuration section. 

### Adding Traces

When you add Distributed tracing using any of the available extension methods, wavefront traces are also sent if the Wavefront configuration is added. 
You can also use the Generic Host or Service Collection extension methods. 
```csharp
    WebHost.CreateDefaultBuilder(args)
                  ...
                  .AddDistributedTracingAspNetCore()
``` 
### Wavefront Configuration 

You can report to Tanzu Observability using either Direct Ingestion or via a proxy.

### Direct ingestion
 For direct ingestion, set the `uri` and `apiToken` as follows: 

 ```json
   "management": {
      "export": {
        "wavefront": {
          "uri": "https://vmware.wavefront.com",
          "apiToken": "<your-api-token>",
          "step": "5000"
        }
      }
    }
  },
  "wavefront": {
    "application": {
      "name": "SteeltoeApp",
      "service": "FortuneTeller-Service"
    }
  }
  ```
  `step` refers to the duration of the reporting interval. In this case traces and/or metrics will be sent every 5 seconds. 

### Proxy
For sending by [proxy](https://docs.wavefront.com/proxies.html), you can run a proxy locally:

```bash
docker run -d -e WAVEFRONT_URL=https://vmware.wavefront.com/api   -e WAVEFRONT_TOKEN=<Your-Wavefront-Token>  -p 2878:2878   wavefronthq/proxy:latest
```

In this case the `uri` would be "proxy://localhost:2878" and `apiToken` is not required.

## Building & Running - Locally

1. Clone this repository. (i.e. git clone <https://github.com/SteeltoeOSS/Samples>)
1. cd samples/Management/src/Tracing/Fortune-Teller-Service
1. dotnet run -f net6.0

## What to expect - Locally

After building and running the app, you should see something like the following:

```bash
$ cd samples/Management/src/Tracing/Fortune-Teller-Service
$ dotnet run -f netcoreapp3.1
info: Microsoft.Data.Entity.Storage.Internal.InMemoryStore[1]
      Saved 50 entities to in-memory store.
Hosting environment: Production
Now listening on: http://*:5000
Application started. Press Ctrl+C to shut down.
```

At this point the Fortune Teller Service is up and running and ready for the Fortune Teller UI to ask for fortunes. 
Go to the Fortune-Teller-UI directory for details on how to start it up.
