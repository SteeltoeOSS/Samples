# SteelToe MusicStore Sample Application
This repo tree contains a sample app illustrating how to use all of the SteelToe components together in a ASP.NET Core application. This application is based on the ASP.NET Core reference app [MusicStore](https://github.com/aspnet/MusicStore) provided by Microsoft.

In creating this application, we took the Microsoft reference application and broke it up into multiple independent services:
* MusicStoreService - provides a RESTful API to the MusicStore and its backend Music database.
* MusicStoreUI - provides the UI to the MusicStore application and all of its services.
* OrderService - provides a RESTful API for Order processing service and its backend Order database. 
* ShoppingCartService - provides a RESTful api to a ShoppingCart service and its backend ShoppingCart database.

Note: The OrderService and ShoppingCartService are independent from the Music application and could be used in any other application requiring those basic services.

# Getting Started

* Clone the Samples repo. (i.e.  git clone https://github.com/SteelToeOSS/Samples)

## Pre-requisites - Local

1. Installed .NET Core SDK.
2. Running instances of the following services on your local machine:

* Spring Cloud Config Server - @ `http://localhost:8888` 
* Spring Cloud Eureka Server - @ `http://localhost:8761/eureka/`
* MySql Database Server - @ `localhost:3306` username: `root`, password: `steeltoe`
* Redis Cache - Optional, can be used for Session state backing store cache.

You have a three options to choose from in order to get these services up and running locally:

* Use pre-built SteelToe Docker images together with [Docker for Windows](https://docs.docker.com/docker-for-windows/). 
* Use pre-built SteelToe Windows Container images. (Note: Windows containers is still in Beta and as such consider it experimental at best)
* Install each service manually.

Currently, the simplest way to get these up and running is to use the first option above together with the provided `dockerrun-*.cmd` files to startup those services.  

### Pre-requisites - Using Docker for Windows

If you don't have [Docker for Windows](https://docs.docker.com/docker-for-windows/) installed, you can follow these [simple instructions]() for installing and setting it up.

Once you have docker installed and running you can use the provided command files to startup the various services.  For example to startup a Spring Cloud Config Server:

1. cd Samples\MusicStore
2. start dockerrun-configserver.cmd

This will create a directory `\steeltoe\config-repo` if it doesn't exist and then fire up a Spring Cloud Config Server listening on port 8888. The Config Server has been pre-configured to read its configuration data from `\steeltoe\config-repo`.

Likewise to startup a Spring Cloud Eureka Server:

1. cd Samples\MusicStore
2. start dockerrun-eurekaserver.cmd

This will fire up a Spring Cloud Eureka Server listening on port 8761.

And finally to startup a MySql Server:

1. cd Samples\MusicStore
2. start dockerrun-mysqlserver.cmd

This will fire up a MySql Server listening on port `3306` with username: `root` and password: `steeltoe`.

### Pre-requisites - Using Windows Containers
Details to be provided!

### Pre-requisites - Install Manually
Details to be provided!

# Building & Running App - Local

Once you have the pre-requisites up and running then you are ready to build and run the various application services locally. Before starting up any of the services you first need to copy the MusicStore configuration files to the `\steeltoe\config-repo' so the running Config Server will have access to them.

1. cd Samples\MusicStore\config
2. copy *.* \steeltoe\config-repo

Once thats complete, then you are ready to fire up the individual services. The simplest way to get these up and running is to use the provided `run-*.cmd` files.

For example, to startup the MusicStoreService simply:

1. cd Samples\MusicStore
2. runMusicStoreService.cmd

Its probably best to startup the MusicStoreService, OrderService and ShoppingCartService first and then follow up with the MusicStoreUI last.

If all the services startup cleanly, you should be able to hit: http://localhost:5555/ to see the Music Store.

# Pre-requisites - CloudFoundry

1. Install Pivotal CloudFoundry 1.7
2. Install Spring Cloud Services 1.0.11.
3. Install .NET Core SDK.
4. Web tools installed and on Path.  If you have VS2015 Update 3 installed then add this to your path: `C:\Program Files (x86)\Microsoft Visual Studio 14.0\Web\External`

# Setup Services on CloudFoundry
As mentioned above, the application is dependent on the following services:
* Spring Cloud Config Server 
* Spring Cloud Eureka Server 
* MySql Database Server 
* Redis Cache - Optional, can be used for Session state backing store cache.

Before pushing the application to CloudFoundry we need to create those services:
1. cf target -o myOrg -s mySpace
2. cd Samples\MusicStore
3. start createCloudFoundryServices.cmd

This will create all of the services needed by the application.  It creates:
* mStoreConfig - Spring Cloud Config Server instance
* mStoreRegistry - Spring Cloud Eureka Server instance
* mStoreAccountsDB - MySql database instance for Users and Roles
* mStoreOrdersDB - MySql database instance for Orders
* mStoreCartDB - MySql database instance for ShoppingCarts
* mStoreStoreDB - MySql database instance for Music

Note: The Spring Cloud Config Server instance created by the above script configures the Config Server instance to use the git repo: https://github.com/SteelToeOSS/musicStore-config.git.  This repo contains the same configuration files as those found in `Samples\MusicStore\config`.
No changes are required to the application configuration files before pushing the app to CloudFoundry.

# Building & Pushing App - CloudFoundry

Once the services have been created on CloudFoundry then you can use the provided `push*.cmd` to startup the individual application services on CloudFoundry. For example to start the ShoppingCart service:

1. cd Samples\MusicStore
2. pushShoppingCartService.cmd




