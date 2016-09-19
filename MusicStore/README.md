# Steeltoe MusicStore Sample Application
This repo tree contains a sample app illustrating how to use all of the Steeltoe components together in a ASP.NET Core application. This application is based on the ASP.NET Core reference app [MusicStore](https://github.com/aspnet/MusicStore) provided by Microsoft.

In creating this application, we took the Microsoft reference application and broke it up into multiple independent services:
* MusicStoreUI - provides the UI to the MusicStore application and all of its services.
* MusicStoreService - provides a RESTful API to the MusicStore and its backend Music database.
* OrderService - provides a RESTful API for Order processing service and its backend Order database. 
* ShoppingCartService - provides a RESTful api to a ShoppingCart service and its backend ShoppingCart database.

Note: The OrderService and ShoppingCartService are independent from the Music application and could be used in any other application requiring those basic services.

This application makes use of the following Steeltoe components:
* Spring Cloud Config Server Client for centralized application configuration
* Spring Cloud Eureka Server Client for service discovery
* Steeltoe Connectors for connecting to MySql using EF6 OR Postgres using EFCore 
* Optionally using Steeltoe Connector to connect to a Redis cache for Session storage.

Note: The MySql and Redis connectors only support .NET 451+ and as such when using them you must target a windows runtime. 
The Postgres connector supports both .NET 451+ and .NET Core.  The default for the application is to use MySql when running locally on Windows and to use Postgres when running on MacOS/Linux.
Also, the default is to not use Redis cache.

# Getting Started

* Clone the Samples repo. (i.e.  git clone https://github.com/SteeltoeOSS/Samples)

## Pre-requisites - Local

1. Installed .NET Core SDK.
2. You need running instances of the following services on your local machine:

* Spring Cloud Config Server - @ `http://localhost:8888` 
* Spring Cloud Eureka Server - @ `http://localhost:8761/eureka/`
* MySql Database Server - @ `localhost:3306` username: `root`, password: `steeltoe` or Postgres Database Server @ `localhost:5432` username: `steeltoe`, password: `steeltoe`
* Redis Cache - Optional, can be used for Session state backing store.

You have a three options to choose from in order to get these services up and running locally:

* Use pre-built Steeltoe Docker images. 
* On Windows 10, use pre-built Steeltoe Windows Container images. (Note: Windows containers on Windows 10 and Windows Server 2016 is still in Beta. Consider it experimental)
* Install each service manually.

Currently, the simplest way to get these up and running is to use the first option above together with the provided `dockerrun-*.cmd, dockerrun-*.sh` files to startup those services.  

### Pre-requisites - Using Steeltoe Docker images

If you don't have Docker installed on your local machine, you can use:
* Windows 10 - [Docker for Windows](https://docs.docker.com/docker-for-windows/) 
* MacOS - [Docker for Mac](https://docs.docker.com/docker-for-mac/)

After installing either of the above, you need to enable [`File Sharing`](https://docs.docker.com/docker-for-windows/#/shared-drives) between your Docker VM and your local machine. 
On Windows 10, share the `C:` drive from you local machine with the VM. On MacOS, share your Home directory with the VM. This is necessary as the Config Server has been pre-configured to read its configuration data from `~/steeltoe/config-repo` or `C:\steeltoe\config-repo` off of the local machine.

Once you have Docker installed and running you can use the provided command files to startup the various services.  For example to startup a Spring Cloud Config Server:

1. cd Samples/MusicStore
2. start dockerrun-configserver.cmd or ./dockerrun-configserver.sh

This will create a directory `~/steeltoe/config-repo` or `c:\steeltoe\config-repo` if it doesn't exist and then fire up a Spring Cloud Config Server listening on port 8888. 

Likewise to startup a Spring Cloud Eureka Server:

1. cd Samples/MusicStore
2. start dockerrun-eurekaserver.cmd or ./dockerrun-eurekaserver.sh

This will fire up a Spring Cloud Eureka Server listening on port 8761.

And finally to startup a MySql Server.  Note: On MacOS you can NOT use MySQL. Instead, you must use Postgres as there currently are no MySql .NET providers supported on .NET Core. 

1. cd Samples/MusicStore
2. start dockerrun-mysqlserver.cmd or ./dockerrun-mysqlserver.sh

This will fire up a MySql Server listening on port `3306` with username: `root` and password: `steeltoe`.

To startup a Postres Server: 

1. cd Samples/MusicStore
2. start dockerrun-postgresserver.cmd or ./dockerrun-postgresserver.sh

This will fire up a Postgres Server listening on port `5432` with username: `steeltoe` and password: `steeltoe`.

### Pre-requisites - Using Windows Containers
Details to be provided!

# Building & Running MusicStore App - Local

Once you have the pre-requisite services up and running then you are ready to build and run the various application services locally. Before starting up any of the services you first need to copy the MusicStore configuration files to the `\steeltoe\config-repo' so the running Config Server will have access to them.

1. cd Samples/MusicStore/src/config
2. copy *.* c:/steeltoe/config-repo or cp *.* ~/steeltoe/config-repo

Once thats complete, then you are ready to fire up the individual services. The simplest way to get these up and running is to use the provided `run*.cmd or run*.sh` files.

For example, to startup the MusicStoreService simply:

1. cd Samples/MusicStore
2. runMusicStoreService.cmd or ./runMusicStoreService.sh

Its probably best to startup the MusicStoreService, OrderService and ShoppingCartService first and then follow up with the MusicStoreUI last.

If all the services startup cleanly, you should be able to hit: http://localhost:5555/ to see the Music Store.

# Pre-requisites - CloudFoundry

1. Install Pivotal CloudFoundry 1.7
2. Install Spring Cloud Services 1.0.11.
3. Install .NET Core SDK.
4. Install Postgres database service if you want to use Postgress instead of MySQL.
4. Web tools installed and on Path.  On Windows, if you have VS2015 Update 3 installed then add this to your path: `C:\Program Files (x86)\Microsoft Visual Studio 14.0\Web\External`

# Setup Services on CloudFoundry

As mentioned above, the application is dependent on the following services:
* Spring Cloud Config Server 
* Spring Cloud Eureka Server 
* MySql Database Server - Default database used by all MusicStore services.
* Postgres Database Server - Optional, you have to specifically compile a MusicStore service to use Postgres.
* Redis Cache - Optional, can be used for Session state backing store cache.

Before pushing the application to CloudFoundry we need to create those services:

1. cf target -o myOrg -s mySpace
2. cd Samples/MusicStore
3. start createCloudFoundryServices.cmd or ./createCloudFoundryServices.sh

Note: If you wish to use Postgres for one or more of the MusicStore services, you will have to modify the above script.

This will create all of the services needed by the application.  Specifically, it creates:
* mStoreConfig - Spring Cloud Config Server instance
* mStoreRegistry - Spring Cloud Eureka Server instance
* mStoreAccountsDB - MySql database instance for Users and Roles (Identity)
* mStoreOrdersDB - MySql database instance for Orders
* mStoreCartDB - MySql database instance for ShoppingCarts
* mStoreStoreDB - MySql database instance for Music

Note: The Spring Cloud Config Server instance created by the above script configures the Config Server instance to use the git repo: https://github.com/SteeltoeOSS/musicStore-config.git.  This repo contains the same configuration files as those found in `Samples/MusicStore/config`.
No changes are required to the application configuration files before pushing the app to CloudFoundry. 

Note: If you wish to change what github repo the Config server instance uses, you can modify config-server.json before using the `createCloudFoundryServices` script above.

# Building & Pushing App - CloudFoundry

Once the services have been created on CloudFoundry then you can use the provided `push*.cmd or push*.sh` to startup the individual application services on CloudFoundry. For example to start the ShoppingCart service:

1. cd Samples/MusicStore
2. pushShoppingCartService.cmd or ./pushShoppingCartService.sh

Each of the `push*.*` scripts publish the MusicStore service targeting `net451` and `win7-x64`  and then push the MusicStore service using the `manifest-windows.yml` found in the projects directory. If you wish to push to a Linux container, you will have to recompile the application for Postgres and publish the app targeting the `netcoreapp1.0` framework and `ubuntu` runtime.  Then use the `manifest.yml` file when pushing the app services.
