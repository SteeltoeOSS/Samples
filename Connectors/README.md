# Connector Sample Applications

This repo tree contains sample apps illustrating how to use the Steeltoe Connectors for connecting to CloudFoundry services in your ASP.NET 7.x application.
Steeltoe Connectors simplify the coding process of binding to and accessing CloudFoundry-based services.

## ASP.NET Core Samples

* src/CosmosDb - Connect to an Azure CosmosDB database on CloudFoundry.
* src/MongoDb - Connect to a MongoDB database on CloudFoundry.
* src/MySql - Connect to a MySQL database on CloudFoundry.
* src/MySqlEFCore - Connect to a MySQL database through an Entity Framework Core `DbContext` on CloudFoundry.
* src/OAuth - Connect to an OAuth2 security service (e.g. [UAA Server](https://github.com/cloudfoundry/uaa) or [Pivotal Single Signon](https://docs.pivotal.io/p-identity/)) on CloudFoundry.
* src/PostgreSql - Connect to a PostgreSQL database on CloudFoundry or Kubernetes.
* src/PostgreSqlEFCore - Connect to a PostgreSQL database through an Entity Framework Core `DbContext` on CloudFoundry or Kubernetes.
* src/RabbitMQ - Connect to a RabbitMQ server on CloudFoundry.
* src/Redis - Connect a [Microsoft Redis Cache Extension](https://github.com/aspnet/Caching/tree/dev/src/Microsoft.Extensions.Caching.Redis) and/or a [ConnectionMultiplexor](https://github.com/StackExchange/StackExchange.Redis) to a Redis cache on CloudFoundry.
* src/SqlServerEFCore - Connect to a Microsoft SQL Server database through an Entity Framework Core `DbContext` on CloudFoundry.

## Building & Running

See the Readme for instructions on building and running each app.

---

### See the Official [Steeltoe Service Connectors Documentation](https://docs.steeltoe.io/api/v3/connectors/) for a more in-depth walkthrough of the samples and more detailed information.
