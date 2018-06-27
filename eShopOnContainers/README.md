# eShopOnContainers Sample Microservices Application
This repo contains a Pivotal Cloud Foundry port of the ASP.NET Core based app created by Microsoft as an example of a modern ASP.NET Core app that illustrates the Microservices approach. The original repo can be viewed here: https://github.com/dotnet-architecture/eShopOnContainers

Consisting of seven backing services, a SPA and MVC user interfaces and several mobile OS implementations the app is a comprehensive example that also demonstrates resiliency and messaging patterns.

The original sample uses Microsoft SQL Server, Redis, RabbitMQ & IdentityServer. The database has been changed to MySQL to make it easier to run on any Pivotal Cloud Foundry instance.

## Steeltoe

This application makes use of the following Steeltoe components:

* Steeltoe Cloud Foundry Configuration
* Steeltoe Management to enable Actuators
* Steeltoe connectors for RabbitMQ, MongoDb, Redis & MySQL
* Steeltoe Data Protection with Redis

## Getting Started

This application requires several Services be created:

* 1 RabbitMQ Instance `eShopMQ`
* 1 Redis Instance `eShopCache`
* 4 MySQL Instances `eShopIdentityDb` `eShopMarketingDb` `eShopOrderingDb` `eShopCatalogDb`
* 1 MongoDb Instance `eShopDocDb`

```bash
cf create-service p-mysql 100mb eShopIdentityDb
cf create-service p-mysql 100mb eShopMarketingDb
cf create-service p-mysql 100mb eShopOrderingDb
cf create-service p-mysql 100mb eShopCatalogDb

cf create-service p-redis shared-vm eShopCache

cf create-service p-rabbitmq standard eShopMQ

cf create-service mongodb-odb standalone_medium eShopDocDb
```

## Create Routes
Before we can deploy this application we'll have to decide on routes for each of the microservices and configure them in the `appsettings.json` of the IdentityApi. Routes are required upfront as the database migrations will setup the oAuth clients on startup. The SPA and MVC applications will also require the Routes for each API.

```bash
cf create-route my-space example.com --hostname identityapi # identityapi.example.com
cf create-route my-space example.com --hostname marketingapi # marketingapi.example.com
cf create-route my-space example.com --hostname locationapi # locationapi.example.com
cf create-route my-space example.com --hostname basketapi # basketapi.example.com
cf create-route my-space example.com --hostname orderingapi # orderingapi.example.com
cf create-route my-space example.com --hostname catalogapi # catalogapi.example.com
cf create-route my-space example.com --hostname paymentapi # paymentapi.example.com
cf create-route my-space example.com --hostname eshopcontainersmvc # eshopcontainersmvc.example.com
cf create-route my-space example.com --hostname eshopcontainersspa # eshopcontainersspa.example.com
```

Creating the Routes before pushing any of the applications will ensure the Route you want to use is available prior to the database migrations executing. If a Route changes you'll either have to modify the data in `eShopIdentityDb` or re-create the service instance so the migrations can re-run.

## Configure Routes in appSettings
Configured the routes in [IndentityApi's appsettings.json]( https://github.com/Steeltoe/Samples/eShopOnContainers/src/Services/Identity/Identity.API/appsettings.json) to use what was created above:  

```json
"MvcClient": "https://eshopcontainersmvc.example.com",
"SpaClient": "https://eshopcontainersspa.example.com",
"MarketingApiClient":"https://marketingapi.example.com",
"LocationApiClient":"https://locationapi.example.com"
"OrderingApiClient":"https://orderingapi.example.com",
"PaymentApiClient":"https://paymentapi.example.com",
```

Update the Route for the IdentityApi in the [BasketApi's appSettings.json](https://github.com/Steeltoe/Samples/eShopOnContainers/src/Services/Basket/Basket.API/appsettings.json)

```json
"IdentityUrl": "https://identityapi.example.com",
```

Update the Route for the CatalogApi in the [CatalogApi's appSettings.json](https://github.com/Steeltoe/Samples/eShopOnContainers/src/Services/Catalog/Catalog.API/appsettings.json)

```json
"PicBaseUrl": "https://catalogapi.example.com/api/v1/catalog/items/[0]/pic/",
```

Update the Route for the IdentityApi in the [LocationApi's appSettings.json](https://github.com/Steeltoe/Samples/eShopOnContainers/src/Services/Location/Locations.API/appsettings.json)

```json
"IdentityUrl": "https://identityapi.example.com",
```

Update the Route for the IdentityApi and Pictures in the [MarketingApi's appSettings.json](https://github.com/Steeltoe/Samples/eShopOnContainers/src/Services/Marketing/Marketing.API/appsettings.json)

```json
  "IdentityUrl": "https://identityapi.example.com",
  "PicBaseUrl": "https://marketingapi.example.com/api/v1/campaigns/[0]/pic/",
```

Update the Route for the IdentityApi in the [OrderingApi's settings.json](https://github.com/Steeltoe/Samples/eShopOnContainers/src/Services/Marketing/Marketing.API/appsettings.json)

```json
 "IdentityUrl": "https://identityapi.example.com",
```

## Deploy the Applications to Pivotal Cloud Foundry
Deploy the `IdentityApi` first as the migrations will setup the IdentityServer configuration required by most of the applications.

On OSX:
```bash
cd src/Services/Identity/Identity.API
./PushCloudFoundry.sh
```

On Windows:
```cmd
cd src\Services\Identity\Identity.API
.\PushCloudFoundry-Windows.ps1
```
This script will `build`, `publish` and `cf push` the application to PCF environment the cf cli is targeting.

Ensure the migrations are successful and the service starts and is marked as healthy by the platform. You should be able to browse to the route and see the default page for `IdentityServer4`.

The remaining Web Api's can be deployed in any order but the process is the same:

```bash
./eShopOnContainers/src/Services/Ordering/Ordering.API
./eShopOnContainers/src/Services/Ordering/Ordering.BackgroundTasks
./eShopOnContainers/src/Services/Basket/Basket.API
./eShopOnContainers/src/Services/Catalog/Catalog.API
./eShopOnContainers/src/Services/Location/Locations.API
./eShopOnContainers/src/Services/Marketing/Marketing.API
./eShopOnContainers/src/Services/Payment/Payment.API
```

The Web Api's can be tested using Swagger.

Finally deploy one or both of the User Interfaces:

```bash
./eShopOnContainers/src/Web/WebMVC
./eShopOnContainers/src/Web/WebSPA
```

To test the application Register an account and purchase an Item from the Store. Experiment with a location based Marketing campaign.