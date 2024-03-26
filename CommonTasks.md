# Frequently Used Information

This page contains information on basic tasks that are used throughout the Steeltoe Samples. Use this page to quickly get your local development environment up and running.

## Spring Cloud Config Server

### Run SCCS with Docker

The Steeltoe team has built a docker image of a [basic Config server](https://github.com/SteeltoeOSS/Dockerfiles/tree/main/config-server) for an easy experience getting started

To start a config server backed by the Spring Cloud Samples repo:

```bash
docker run --rm -ti -p 8888:8888 --name steeltoe-config steeltoeoss/config-server
```

To start a config server backed by a folder on your local disk, start the docker image like this:

```bash
# Note: Ensure Docker is configured to share host drive/volume so the mount below will work correctly!
docker run --rm -ti -p 8888:8888 -v $PWD/steeltoe/config-repo:/config --name steeltoe-config steeltoeoss/configserver --spring.profiles.active=native
```

### Run SCCS with Java

To run a Spring Cloud Config Server without Docker:

1. Clone the Spring Cloud Config Server repository: `git clone https://github.com/spring-cloud/spring-cloud-config`
1. Change to the directory the server is located in: `cd spring-cloud-config/spring-cloud-config-server`
1. Review the readme and ensure you have the required JDK installed
1. Build the source: `.\mvnw install -DskipTests`
1. Start the server: `.\mvnw spring-boot:run`

The default configuration of the Config Server uses [this github repo](https://github.com/spring-cloud-samples/config-repo) for its source of configuration data.

### Provision SCCS on Cloud Foundry

Use the [cf cli](https://github.com/cloudfoundry/cli) to create a Spring Cloud Config Server in a org/space, backed by a given git repo. Many of the Steeltoe samples use the `spring-cloud-samples` repo, but you may need to alter the parameter used.

1. `cf target -o myorg -s myspace`
1. Use the correct escaping for your shell:
   1. bash or Powershell: `cf create-service p-config-server standard myConfigServerInstanceName -c '{"git":{"uri": "https://github.com/spring-cloud-samples/config-repo"}}'`
   1. CMD: `cf create-service p.config-server standard myConfigServerInstanceName -c "{\"git\":{\"uri\":\"https://github.com/spring-cloud-samples/config-repo\"}}"`
1. Wait for service to be ready. (use `cf services` to check the status)

## Spring Cloud Eureka Server

### Run Eureka with Docker

The Steeltoe team has built a docker image of a [basic Eureka server](https://github.com/SteeltoeOSS/Dockerfiles/tree/main/eureka-server) for an easy experience getting started:

```bash
docker run --rm -ti -p 8761:8761 --name steeltoe-eureka steeltoeoss/eureka-server
```

### Run Eureka with Java

### Provision Eureka on Cloud Foundry

Use the [cf cli](https://github.com/cloudfoundry/cli) to create a Service Registry service in a org/space.

1. cf target -o myorg -s myspace
1. cf create-service p.service-registry standard myDiscoveryServiceInstanceName
1. Wait for service to be ready. (use `cf services` to check the status)

## RabbitMQ

### Run RabbitMQ Server with Docker

This command starts a RabbitMQ server with the management plugin enabled with no credentials and default ports:

```script
docker run --rm -ti -p 5672:5672 -p 15672:15672 --name rabbitmq rabbitmq:3-management
```

## Consul

### Run Consul Server with Docker

```script
docker run --rm -ti -p 8500:8500 --name=steeltoe-consul consul
```

## Spring Boot Admin

### Run Spring Boot Admin Server with Docker

There are multiple Spring Boot Admin images to choose from, this is only one option:

```script
docker run --rm -it -p 8080:8080 --name steeltoe-springbootadmin steeltoeoss/spring-boot-admin
```

## Redis

### Run Redis server with Docker

```script
docker run --rm -ti -p 6379:6379 --name redis redis
```

## MySQL

### Run MySQL Server with Docker

```script
docker run --rm -ti -p 3306:3306 --name steeltoe-mysql -e MYSQL_ROOT_PASSWORD=steeltoe -e MYSQL_DATABASE=steeltoe -e MYSQL_USER=steeltoe -e MYSQL_PASSWORD=steeltoe mysql
```

## SQL Server

### Run SQL Server with Docker

```script
docker run --rm -ti -p 1433:1433 --name steeltoe-sqlserver -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=St33ltoeR0cks!' mcr.microsoft.com/mssql/server
```

## PostgreSQL

### Run PostgreSQL Server with Docker

```script
docker run --rm -ti -p 5432:5432 --name steeltoe-postgres -e POSTGRES_DB=steeltoe -e POSTGRES_USER=steeltoe -e POSTGRES_PASSWORD=steeltoe postgres:alpine
```

## MongoDB

### Run MongoDB Server with Docker

```script
docker run --rm -ti -p 27017:27017 --name mongoserver mongo
```

## Zipkin

### Run Zipkin Server with Docker

```script
docker run --rm -ti -p 9411:9411 --name zipkin openzipkin/zipkin
```
