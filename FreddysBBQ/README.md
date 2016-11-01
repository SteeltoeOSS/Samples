# Freddys BBQ Sample Application
This repo tree contains a polyglot (i.e. Java and .NET) microservices based sample app illustrating interoperability between Java and .NET based microservices running on CloudFoundry, secured with OAuth2 Security Services and using Spring Cloud Services.

To fully understand how this application is structured, you should have a good look at the Java based [Freddys BBQ sample](https://github.com/william-tran/freddys-bbq).

This application makes use of the following Steeltoe components:
* Spring Cloud Config Server Client for centralized application configuration
* Spring Cloud Eureka Server Client for service discovery
* Steeltoe Connectors for connecting to MySql using EF6  
* Steeltoe CloudFoundry Security Provider for SSO and REST endpoint protection

# Getting Started

This repo contains two of the four components that make up Freddys BBQ application  ( i.e.`Admin Portal UI` and `Order REST API`). These two components have been rewriten using .NET and ASP.NET Core and will be used to illustrate interoperability between Java and .NET based microservices running on CloudFoundry.

To proceed you will first deploy the Java version of Freddys BBQ on CloudFoundry. To do this, you should follow the [deployment instructions](https://github.com/william-tran/freddys-bbq) for the Java version of Freddys BBQ first and verify that this version of the application is up and running properly.

Once that is complete then the next step will be to replace the tow Java services, `Admin Portal UI` and `Order REST API` with the .NET versions found in this repo. 

After that is complete you will have a running example of a Java and .NET based microservices based app running on CloudFoundry, secured with OAuth2 Security Services and using Spring Cloud Services.

# Replace Java Services with .NET Services

Once you have finished deploying and verifing the Java version, you are then ready to proceed with this section.

## Add MySQL Order Database

First you will need to create a new MySql service for our .NET version of the Order REST API service. Run the following commands:
```
cf create-service p-mysql 100mb mysql-orders
```
## Update admin-portal SSO Setting
Next, we have to make one small change to `Redirect URIs` for the admin-portal component in the SSO dashboard. To do this we need to access the `SSO` service dashboard. In order to access the dashboard, run the following command and go to the URL listed in `Dashboard` property:

```
$ cf service sso

Service instance: sso
Service: p-identity
Bound apps: customer-portal,menu-service,admin-portal,order-service
Tags:
Plan: auth
Description: Single Sign-On as a Service
Documentation url: http://docs.pivotal.io/p-identity/index.html
Dashboard: https://p-identity.mypcf.example.com/dashboard/identity-zones/{ZONE_GUID}/instances/{INSTANCE_GUID}/
...
```

Once you are in the dashboard select the `admin-portal`.  Next in the `Auth Redirect URIs` box, duplicate current URI appending it to the current one (comma separating it). Then in the duplicated one, replace the `https` with `http` and append `/signin-cloudfoundry` to it. 

So for example, if you started with:
```
https://admin-portal.apps.testcloud.com
```
then after making the changes, the Auth Redirect URIs would look as follows:
```
https://admin-portal.apps.testcloud.com,http://admin-portal.apps.testcloud.com/signin-cloudfoundry
```
Save your changes  .... (i.e. `Save Config`)

## Replace admin-portal & order-service

At this point you are ready to replace the existing Java based services with the .NET versions. To do this enter the following commands:
```
cf delete admin-portal
cf delete order-service
cd Samples\FreddysBBQ\src\OrderService
dotnet restore --configfile nuget.config
dotnet publish -out %CD%\publish -c Debug -f net451 -r win7-x64
cf push -f manifest-windows.yml -p %CD%\publish
cd ..\AdminPortal
dotnet restore --configfile nuget.config
dotnet publish -out %CD%\publish -c Debug -f net451 -r win7-x64
cf push -f manifest-windows.yml -p %CD%\publish
```

At this point the app should continue to work as it did before.  Any orders you might have had before, will be gone as you are now starting with a new clean order database. 
_

