# Frequently Used Information

This page contains information on basic tasks that are used throughout the Steeltoe Samples. Use this page to quickly get your local development environment up and running.

## Spring Cloud Config Server

### Run SCCS with Docker

The Steeltoe team has built a docker image of a [basic Config server](https://github.com/SteeltoeOSS/Dockerfiles/tree/master/config-server) for an easy experience getting started

To start a config server backed by a folder on your local disk, start the docker image like this:

```bash
# Note: Ensure Docker is configured to share host drive/volume so the mount below will work correctly!
docker run --rm -ti -p 8888:8888 -v $PWD/steeltoe/config-repo:/config --name steeltoe-config steeltoeoss/configserver --spring.profiles.active=native
```

To start a config server backed by the spring cloud samples repo:

```bash
docker run --rm -ti -p 8888:8888 --name steeltoe-config steeltoeoss/configserver
```

### Run SCCS with Java

To run a Spring Cloud Config Server without Docker:

1. Install Java 8 JDK.
1. Install Maven 3.x.
1. Clone the Spring Cloud Config Server repository: `git clone https://github.com/spring-cloud/spring-cloud-config`
1. Change to the directory the server is located in: `cd spring-cloud-config/spring-cloud-config-server`
1. Start the server: `mvn spring-boot:run`

The default configuration of the Config Server uses [this github repo](https://github.com/spring-cloud-samples/config-repo) for its source of configuration data.

### Provision SCCS on Cloud Foundry

Use the [cf cli](https://github.com/cloudfoundry/cli) to create a Spring Cloud Config Server in a org/space, backed by a given git repo. Many of the Steeltoe samples use the `spring-cloud-samples` repo, but you may need to alter the parameter used.

1. `cf target -o myorg -s myspace`
1. Use the correct escaping for your shell:
   1. bash: `cf create-service p-config-server standard myConfigServerInstanceName -c '{"git":{"uri": "https://github.com/spring-cloud-samples/config-repo"}}'`
   1. CMD: `cf create-service p-config-server standard myConfigServerInstanceName -c "{\"git\":{\"uri\":\"https://github.com/spring-cloud-samples/config-repo\"}}"`
   1. PowerShell: `cf create-service p-config-server standard myConfigServerInstanceName -c '{\"git\":{\"uri\":\"https://github.com/spring-cloud-samples/config-repo\"}}'`
1. Wait for service to be ready. (use `cf services` to check the status)

## Spring Cloud Eureka Server

### Run Eureka with Docker

The Steeltoe team has built a docker image of a [basic Eureka server](https://github.com/SteeltoeOSS/Dockerfiles/tree/master/eureka-server) for an easy experience getting started:

```bash
docker run --publish 8761:8761 steeltoeoss/eureka-server
```

### Run Eureka with Java

### Provision Eureka on Cloud Foundry

Use the [cf cli](https://github.com/cloudfoundry/cli) to create a Service Registry service in a org/space.

1. cf target -o myorg -s myspace
1. cf create-service p-service-registry standard myDiscoveryServiceInstanceName
1. Wait for service to be ready. (use `cf services` to check the status)

## RabbitMQ

### Run RabbitMQ Server with Docker

This command starts a RabbitMQ server with the management plugin enabled with no credentials and default ports:

```script
docker run --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

## Consul

### Run Consul Server with Docker

```script
docker run --name=steeltoe-consul -p 8500:8500 consul
```

## Spring Boot Admin

### Run Spring Boot Admin Server with Docker

There are multiple Spring Boot Admin images to choose from, this is only one option:

```script
docker run --name --rm -it --name steeltoe-springbootadmin -p 8080:8080 steeltoeoss/spring-boot-admin
```

## Redis

### Run Redis server with Docker

```script
docker run --name redis -p 6379:6379 redis
```

## MySQL

### Run MySQL Server with Docker

```script
docker run --name steeltoe-mysql -p 3306:3306 -e MYSQL_ROOT_PASSWORD=steeltoe -e MYSQL_DATABASE=steeltoe -e MYSQL_USER=steeltoe -e MYSQL_PASSWORD=steeltoe mysql
```

## SQL Server

### Run SQL Server with Docker

```script
docker run --name steeltoe-sqlserver-e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=St33ltoeR0cks!' -p 1433:1433 mcr.microsoft.com/mssql/server
```

## PostgreSQL

### Run PostgreSQL Server with Docker

```script
docker run --name steeltoe-postgres -d -p 5432:5432 -e POSTGRES_DB=steeltoe -e POSTGRES_USER=steeltoe -e POSTGRES_PASSWORD=steeltoe postgres:alpine
```

## Zipkin

### Run Zipkin Server with Docker

```script
docker run --name zipkin -p 9411:9411 openzipkin/zipkin
```

## Hystrix Dashboard

### Run Hystrix Dashboard with Docker

There are a few images available on Docker Hub that provide basic Hystrix Dashboard functionality. This example has been tested:

```bash
docker run --rm -ti -p 7979:7979 --name steeltoe-hystrix kennedyoliveira/hystrix-dashboard
```

Once this image is up and running, you should be able to browse to your [local dashboard](http://localhost:7979/hystrix-dashboard/) and provide the address of the Hystrix stream(s) you wish to monitor.

> NOTE: This image may be running on a separate network than your application. Remember to provide a stream address that is accessible from within the Docker network. This may require using the external IP address of your workstation or the name of the machine instead of 127.0.0.1 or localhost.

### Run Hystrix Dashboard with Java

To run a Hystrix Dashboard without Docker:

1. Install Java 8 JDK.
1. Install Maven 3.x.
1. Clone the Spring Cloud Samples Hystrix dashboard: `cd https://github.com/spring-cloud-samples/hystrix-dashboard`
1. Change to the hystrix dashboard directory: `cd hystix-dashboard`
1. Start the server `mvn spring-boot:run`
1. Open a browser window and connect to the dashboard: <http://localhost:7979>
1. In the first field, enter the endpoint that is exposing the hystrix metrics (eg: <http://localhost:5555/hystrix/hystrix.stream>)
1. Click the monitor button.
1. Interact with the application to trigger usage of the circuits. Observe the values changing in the Hystrix dashboard.

### Provision Hystrix Dashboard on Cloud Foundry

Use the [cf cli](https://github.com/cloudfoundry/cli) to create a Circuit Breaker service in a org/space.

1. cf target -o myorg -s development
1. cf create-service p-circuit-breaker-dashboard standard myHystrixServiceInstanceName
1. Wait for the service to become ready! (use `cf services` to check the status)
