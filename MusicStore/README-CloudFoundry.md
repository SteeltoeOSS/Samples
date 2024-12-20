# Running the Music Store on Cloud Foundry

## Pre-requisites - CloudFoundry

1. Install Spring Cloud Services
1. Install Redis service - for sharing Session and KeyRing storage between app instances.

## Setup Services on CloudFoundry

The application is dependent on the following services:

* Spring Cloud Config Server
* Spring Cloud Eureka Server
* MySql Database Server - Default database used by all MusicStore services.
* Redis Cache - Optional! Note you have to specifically build/publish MusicStoreUI service to use Redis (Details below).

> Note: Redis Cache is required if you want to scale the MusicStoreUI app to multiple instances (e.g. cf scale musicui -i 2+). Redis is not required to scale the other microservices.

Before pushing the application to CloudFoundry you need to create those services.  If you plan on using Redis, set the environment variable USE_REDIS_CACHE=true before running these command.

> Note: MySQL v2 uses different naming conventions by default than v1. If your environment uses MySQL v2, you may need to alter the createCloudFoundryServices script accordingly!

1. `cf target -o myOrg -s mySpace`
1. `cd Samples/MusicStore`
1. Optionally - `SET USE_REDIS_CACHE=true` or `export USE_REDIS_CACHE=true`
1. `start createCloudFoundryServices.cmd` or `./createCloudFoundryServices.sh`

This will create all of the services needed by the application.  Specifically, it creates:

* mStoreConfig - Spring Cloud Config Server instance
* mStoreRegistry - Spring Cloud Eureka Server instance
* mStoreAccountsDB - MySql database instance for Users and Roles (Identity)
* mStoreOrdersDB - MySql database instance for Orders
* mStoreCartDB - MySql database instance for ShoppingCarts
* mStoreStoreDB - MySql database instance for MusicStore
* mStoreRedis(optionally) - Redis cache instance used by MusicStoreUI for storing Session state

> Note: The Spring Cloud Config Server instance created by the above script configures the Config Server instance to use the git repo: <https://github.com/SteeltoeOSS/musicStore-config.git>.  This repo contains the same configuration files as those found in `Samples/MusicStore/config`.
No changes are required to the application configuration files before pushing the app to CloudFoundry.

> Note: If you wish to change what github repo the Config server instance uses, you can modify config-server.json before using the `createCloudFoundryServices` script above.

## Building & Pushing App - CloudFoundry

Once the services have been created and ready on CloudFoundry (i.e. check via `cf services`) then you can use the provided `push*.cmd or push*.sh` commands to startup the individual application services on CloudFoundry. For example to start the ShoppingCart service:

1. `cd Samples/MusicStore`
1. `pushShoppingCartService.cmd win10-x64 netcoreapp2.1` or `./pushShoppingCartService.sh ubuntu.16.04-x64 netcoreapp2.1`

Note: If you wish to use the Redis cache for storing Session state, you will have to set ENVIRONMENT variable `DefineConstants=USE_REDIS_CACHE` before building and pushing the MusicUI application.

Each of the `push*.*` scripts `dotnet publish` the MusicStore service targeting the `framework` and `runtime` you specify.  They then push the MusicStore service using the appropriate CloudFoundry manifest found in the projects directory (e.g. `manifest-windows.yml`, `manifest.yml` ).

Note: If you are using self-signed certificates it is possible that you might run into SSL certificate validation issues when pushing these apps. The simplest way to fix this:

1. Disable certificate validation for the Spring Cloud Config Client.  You can do this by editing `appsettings.json` and add `spring:cloud:config:validate_certificates=false`. You will need to do this for each of the applications.

Once you have pushed all the applications to Cloud Foundry, if you do a `cf a`, you should see the following applications:

* musicui - Music store User Interface
* musicstore - Music store database micro-service
* orderprocessing - Order processing micro-service
* shoppingcart - Shopping cart micro-service

```shell
name              requested state   instances   memory   disk   urls
musicstore        started           1/1         1G       1G     musicstore.apps.testcloud.com
musicui           started           1/1         1G       1G     musicui.apps.testcloud.com
orderprocessing   started           1/1         1G       1G     orderprocessing.apps.testcloud.com
shoppingcart      started           1/1         1G       1G     shoppingcart.apps.testcloud.com
```

## Known Limitations

## Sample Databases

All MusicStore services (i.e. MusicStoreUI, OrderService, etc.) have their own database instance for persisting data.  When a MusicStore service is started locally, it will always drop and recreate its database upon startup. When a MusicStore service is started on CloudFoundry, only the first instance (i.e. CF_INSTANCE_INDEX=0) will drop and recreate its database.  Note then, the service is not fully ready until the first instance has finished initializing its database, even though other instances are ready.
