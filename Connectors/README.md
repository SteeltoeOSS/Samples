# Connector Sample Applications

This repo tree contains sample apps illustrating how to use the Steeltoe Connectors for connecting to Cloud Foundry services in your ASP.NET 8.x application.
Steeltoe Connectors simplify the coding process of binding to and accessing Cloud Foundry-based services.

## ASP.NET Core Samples

* src/CosmosDb - Connect to an Azure CosmosDB database on Cloud Foundry.
* src/MongoDb - Connect to a MongoDB database on Cloud Foundry or Kubernetes.
* src/MySql - Connect to a MySQL database on Cloud Foundry or Kubernetes.
* src/MySqlEFCore - Connect to a MySQL database through an Entity Framework Core `DbContext` on Cloud Foundry or Kubernetes.
* src/PostgreSql - Connect to a PostgreSQL database on Cloud Foundry or Kubernetes.
* src/PostgreSqlEFCore - Connect to a PostgreSQL database through an Entity Framework Core `DbContext` on Cloud Foundry or Kubernetes.
* src/RabbitMQ - Connect to a RabbitMQ server on Cloud Foundry or Kubernetes.
* src/Redis - Connect to a Redis cache on Cloud Foundry or Kubernetes and configure [IDistributedCache](https://www.nuget.org/packages/Microsoft.Extensions.Caching.Redis).
* src/SqlServerEFCore - Connect to a Microsoft SQL Server database through an Entity Framework Core `DbContext` on Cloud Foundry.

## Building & Running

See the Readme for instructions on building and running each app.

---

See the Official [Steeltoe Connectors Documentation](https://docs.steeltoe.io/api/v4/connectors/) for more detailed information.
