# Connector Sample Applications
This repo tree contains sample apps illustrating how to use the Steeltoe Connectors for connecting to CloudFoundry services in your ASP.NET Core and/or ASP.NET 4.x application. Steeltoe Connectors simplify the coding process of binding to and accessing CloudFoundry based services.
 
* src/AspDotNetCore/MySql - ASP.NET Core sample app illustrating how to use [MySql Connector](https://github.com/SteeltoeOSS/Connectors/tree/master/src/Steeltoe.CloudFoundry.Connector.MySql) to connect your application to a MySql database on CloudFoundry.
* src/AspDotNetCore/MySqlEF6 - ASP.NET Core sample app illustrating how to use [MySql Connector](https://github.com/SteeltoeOSS/Connectors/tree/master/src/Steeltoe.CloudFoundry.Connector.MySql) to connect a EntityFramework6 `DbContext` to a MySql database on CloudFoundry.
* src/AspDotNetCore/Redis - ASP.NET Core sample app illustrating how to use [Redis](https://github.com/SteeltoeOSS/Connectors/tree/master/src/Steeltoe.CloudFoundry.Connector.Redis) to connect a [Microsoft Redis Cache Extension](https://github.com/aspnet/Caching/tree/dev/src/Microsoft.Extensions.Caching.Redis) and/or a [ConnectionMultiplexor](https://github.com/StackExchange/StackExchange.Redis) to a Redis cache on CloudFoundry.
* src/AspDotNetCore/PostgreEFCore - ASP.NET Core sample app illustrating how to use [PostgreEFCore](https://github.com/SteeltoeOSS/Connectors/tree/master/src/Steeltoe.CloudFoundry.Connector.PostgreSql)to connect a EntityFramework Core `DbContext` to a Postgres database on CloudFoundry.
* src/AspDotNetCore/PostgreSql - ASP.NET Core sample app illustrating how to use [PostgreSql](https://github.com/SteeltoeOSS/Connectors/tree/master/src/Steeltoe.CloudFoundry.Connector.PostgreSql) to connect your application to a Postgres database on CloudFoundry.
* src/AspDotNetCore/OAuth - ASP.NET Core sample app illustrating how to use [OAuth](https://github.com/SteeltoeOSS/Connectors/tree/master/src/Steeltoe.CloudFoundry.Connector.OAuth) to connect your application to a OAuth2 security service (e.g. [UAA Server](https://github.com/cloudfoundry/uaa) or [Pivotal Single Signon](https://docs.pivotal.io/p-identity/)) on CloudFoundry.

# Building & Running
See the Readme for instructions on building and running each app.
