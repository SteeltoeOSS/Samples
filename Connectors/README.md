# Connector Sample Applications

This repo tree contains sample apps illustrating how to use the Steeltoe Connectors for connecting to Cloud Foundry services in your ASP.NET application.
Steeltoe Connectors simplify the coding process of binding to and accessing Cloud Foundry-based services.

## ASP.NET Core Samples

* [CosmosDb](src/CosmosDb/README.md) - Connect to an Azure CosmosDB database on Cloud Foundry.
* [MongoDb](src/MongoDb/README.md) - Connect to a MongoDB database on Cloud Foundry or Kubernetes.
* [MySql](src/MySql/README.md) - Connect to a MySQL database on Cloud Foundry or Kubernetes.
* [MySqlEFCore](src/MySqlEFCore/README.md) - Connect to a MySQL database through an Entity Framework Core `DbContext` on Cloud Foundry or Kubernetes.
* [PostgreSql](src/PostgreSql/README.md) - Connect to a PostgreSQL database on Cloud Foundry or Kubernetes.
* [PostgreSqlEFCore](src/PostgreSqlEFCore/README.md) - Connect to a PostgreSQL database through an Entity Framework Core `DbContext` on Cloud Foundry or Kubernetes.
* [RabbitMQ](src/RabbitMQ/README.md) - Connect to a RabbitMQ server on Cloud Foundry or Kubernetes.
* [Redis](src/Redis/README.md) - Connect to a Redis cache on Cloud Foundry or Kubernetes and configure [IDistributedCache](https://www.nuget.org/packages/Microsoft.Extensions.Caching.Redis).
* [SqlServerEFCore](src/SqlServerEFCore/README.md) - Connect to a Microsoft SQL Server database through an Entity Framework Core `DbContext` on Cloud Foundry.

## Building & Running

See the individual sample READMEs linked above for instructions on building and running each app.

---

See the Official [Steeltoe Connectors Documentation](https://docs.steeltoe.io/api/v4/connectors/) for more detailed information.
