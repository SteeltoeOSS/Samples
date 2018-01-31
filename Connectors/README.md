# Connector Sample Applications

This repo tree contains sample apps illustrating how to use the Steeltoe Connectors for connecting to CloudFoundry services in your ASP.NET Core and/or ASP.NET 4.x application. Steeltoe Connectors simplify the coding process of binding to and accessing CloudFoundry based services.

## ASP.NET 4.x Samples

* src/AspDotNet4/MsSql4 - connect with Entity Framework 6 to Microsoft SQL Server on CloudFoundry
* src/AspDotNet4/MySql4 - connect to a MySQL database on CloudFoundry

## ASP.NET Core Samples

* src/AspDotNetCore/MySql - connect to a MySql database on CloudFoundry.
* src/AspDotNetCore/MySqlEF6 - connect an EntityFramework6 `DbContext` to a MySql database on CloudFoundry.
* src/AspDotNetCore/OAuth - connect to an OAuth2 security service (e.g. [UAA Server](https://github.com/cloudfoundry/uaa) or [Pivotal Single Signon](https://docs.pivotal.io/p-identity/)) on CloudFoundry.
* src/AspDotNetCore/PostgreEFCore - connect an EntityFramework Core `DbContext` to a PostgreSQL database on CloudFoundry.
* src/AspDotNetCore/PostgreSql - connect to a Postgres database on CloudFoundry.
* src/AspDotNetCore/RabbitMQ - connect to RabbitMQ on CloudFoundry
* src/AspDotNetCore/Redis - connect a [Microsoft Redis Cache Extension](https://github.com/aspnet/Caching/tree/dev/src/Microsoft.Extensions.Caching.Redis) and/or a [ConnectionMultiplexor](https://github.com/StackExchange/StackExchange.Redis) to a Redis cache on CloudFoundry.
* src/AspDotNetCore/SqlServerEFCore - connect an EntityFramework Core `DbContext` to a Microsoft SQL database on CloudFoundry.

## Building & Running

See the Readme for instructions on building and running each app.

---

### See the Official [Steeltoe Service Connectors Documentation](https://steeltoe.io/docs/steeltoe-service-connectors) for a more in-depth walkthrough of the samples and more detailed information