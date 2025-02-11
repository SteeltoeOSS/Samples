# Steeltoe Sample Applications

This repository contains a variety of sample applications illustrating how to use the Steeltoe framework:

## [Configuration](Configuration)

Samples using the Spring Cloud Config Server and other Steeltoe configuration providers.

| Sample | Status |
| --- | --- |
| [ConfigurationProviders](Configuration/src/ConfigurationProviders) | [![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status%2FSamples%2FConfiguration)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=73) |

## [Discovery](Discovery)

Samples using Steeltoe Service Discovery microservices-based applications.

## [Management](Management/src)

Samples using the Steeltoe Management packages for adding Management REST endpoints to your application, as well as adding Distributed Tracing support.

| Sample | Status |
| --- | --- |
| [ActuatorWeb](./Management/src/ActuatorWeb/) | [![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Samples/SteeltoeOSS.Samples%20%5BManagement_CloudFoundry%5D)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=23) |

## [Connectors](Connectors)

Samples using the Steeltoe Connectors for connecting to backing services. Steeltoe Connectors simplify the coding process of binding to and accessing Cloud Foundry-based services.

| Sample | Status |
| --- | --- |
| [CosmosDb](Connectors/src/CosmosDb) | [![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Samples/Steeltoe-Samples-Connectors-CosmosDb)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=69) |
| [MongoDb](Connectors/src/MongoDb) | [![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Samples/Steeltoe-Samples-Connectors-MongoDb)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=70) |
| [MySql](Connectors/src/MySql) | [![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Samples/Steeltoe-Samples-Connectors-MySql)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=17) |
| [MySqlEFCore](Connectors/src/MySqlEFCore) | [![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Samples/Steeltoe-Samples-Connectors-MySqlEFCore)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=18) |
| [PostgreSql](Connectors/src/PostgreSql) | [![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Samples/Steeltoe-Samples-Connectors-PostgreSql)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=21) |
| [PostgreSqlEFCore](Connectors/src/PostgreSqlEFCore) | [![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Samples/Steeltoe-Samples-Connectors-PostgreSqlEFCore)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=22) |
| [RabbitMQ](Connectors/src/RabbitMQ) | [![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Samples/Steeltoe-Samples-Connectors-RabbitMQ)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=14) |
| [Redis](Connectors/src/Redis) | [![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Samples/Steeltoe-Samples-Connectors-Redis)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=20) |
| [SqlServerEFCore](Connectors/src/SqlServerEFCore) | [![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Samples/Steeltoe-Samples-Connectors-SqlServerEFCore)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=71) |

## [Security](Security)

Samples using the Steeltoe Security packages for Authentication and Authorization with Cloud Foundry auth services and using a Redis cache for DataProtection KeyRing storage.

| Sample | Status |
| --- | --- |
| [RedisDataProtection](Security/src/RedisDataProtection) | [![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status%2FSamples%2FSteeltoe-Samples-Security-RedisDataProtection)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=74) |

# Branches

All new development is done on the main branch. Samples for 2.x and 3.x version of Steeltoe can be found in branches 2.x and 3.x.

# Documentation

If you are looking for documentation on how to use the Steeltoe components, you can find that [here](https://steeltoe.io/docs/).

# Building & Running

See the Readmes for each sample for instructions on how to build and run.
