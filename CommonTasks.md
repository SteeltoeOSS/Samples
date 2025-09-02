# Frequently Used Information

This page contains information on basic tasks that are used throughout the Steeltoe Samples. Use this page to quickly get your local development environment up and running.

Container images produced by the Steeltoe team (all images hosted at `steeltoe.azurecr.io`) are *for local development purposes only*. Images are generally very basic and formal support should not be expected. To see how the images are built and/or participate in improving them, visit [this repository](https://github.com/SteeltoeOSS/Dockerfiles).

Feel free to modify these commands as needed, knowing that commands provided in this document will result in:

* The latest available (tag-matching) image being used: `--pull=always`
* Servers that are automatically removed when the container is stopped: `--rm`
* An attached pseudo-TTY with STDIN that is kept open: `-it` [read more](https://docs.docker.com/reference/cli/docker/container/run/#tty)
* Published ports: `-p <hostPort>:<containerPort>`
* Named containers: `--name <name>`. If the command has Steeltoe-specific configuration and/or the image was built by the Steeltoe team, the name starts with 'steeltoe-'

## Spring Cloud Config Server

### Run SCCS with Docker

To start a config server backed by the Spring Cloud Samples repo:

```shell
docker run --rm -it --pull=always -p 8888:8888 --name steeltoe-config steeltoe.azurecr.io/config-server
```

To start a config server backed by a folder on your local disk, start a Docker container like this:

```shell
# Note: Ensure Docker is configured to share host drive/volume so the mount below will work correctly!
docker run --rm -it --pull=always -p 8888:8888 -v $PWD/steeltoe/config-repo:/config --name steeltoe-config steeltoe.azurecr.io/config-server --spring.profiles.active=native
```

To start a config server that registers itself with Eureka at startup (discovery-first):

```shell
docker run --rm -it --pull=always -p 8888:8888 --name steeltoe-config -e eureka.client.enabled=true steeltoe.azurecr.io/config-server
```

To start a config server that requires basic authentication (and change username and password from the defaults of devuser/devpassword):

```shell
docker run --rm -it --pull=always -p 8888:8888 --name steeltoe-config steeltoe.azurecr.io/config-server --auth.enabled=true --auth.username=username --auth.password=password
```

### Run SCCS with Java

To run a Spring Cloud Config Server without Docker:

1. Clone the Spring Cloud Config Server repository: `git clone https://github.com/spring-cloud/spring-cloud-config`
1. Change to the directory the server is located in: `cd spring-cloud-config/spring-cloud-config-server`
1. Review the readme and ensure you have the required JDK installed
1. Build the source: `.\mvnw install -DskipTests`
1. Start the server: `.\mvnw spring-boot:run`

The default configuration of the Config Server uses [this GitHub repo](https://github.com/spring-cloud-samples/config-repo) for its source of configuration data.

### Provision SCCS on Cloud Foundry

Use the [Cloud Foundry CLI](https://github.com/cloudfoundry/cli) to create a Spring Cloud Config Server instance in an org/space, backed by a given git repo. Many of the Steeltoe samples use the `spring-cloud-samples` repo, but you may need to alter the parameter used.

1. Choose a service plan
   ```shell
   cf target -o your-org -s your-space
   cf marketplace
   cf marketplace -e your-offering
   ```
1. Use the correct escaping for your shell:
   1. bash or PowerShell: `cf create-service p.config-server your-plan sampleConfigServer -c '{"git":{"uri": "https://github.com/spring-cloud-samples/config-repo"}}'`
   1. cmd: `cf create-service p.config-server your-plan sampleConfigServer -c "{\"git\":{\"uri\":\"https://github.com/spring-cloud-samples/config-repo\"}}"`
1. Wait for the service to be ready (use `cf services` to check the status)

## Spring Cloud Eureka Server

### Run Eureka with Docker

```shell
docker run --rm -it --pull=always -p 8761:8761 --name steeltoe-eureka steeltoe.azurecr.io/eureka-server
```

### Provision Eureka on Cloud Foundry

Use the [Cloud Foundry CLI](https://github.com/cloudfoundry/cli) to create a Service Registry instance in an org/space.

1. Choose a service plan
   ```shell
   cf target -o your-org -s your-space
   cf marketplace
   cf marketplace -e your-offering
   ```
1. `cf create-service p.service-registry your-plan sampleDiscoveryService`
1. Wait for the service to be ready (use `cf services` to check the status)

## Spring Boot Admin

### Run Spring Boot Admin Server with Docker

```shell
docker run --rm -it --pull=always -p 9099:9099 --name steeltoe-SpringBootAdmin steeltoe.azurecr.io/spring-boot-admin
```

## HashiCorp Consul

### Run HashiCorp Consul Server with Docker

```shell
docker run --rm -it --pull=always -p 8500:8500 --name consul hashicorp/consul
```

## MongoDB

### Run MongoDB Server with Docker

```shell
docker run --rm -it --pull=always -p 27017:27017 --name mongo mongo
```

## MySQL

### Run MySQL Server with Docker

```shell
docker run --rm -it --pull=always -p 3306:3306 --name steeltoe-mysql -e MYSQL_ROOT_PASSWORD=steeltoe -e MYSQL_DATABASE=steeltoe -e MYSQL_USER=steeltoe -e MYSQL_PASSWORD=steeltoe mysql
```

## PostgreSQL

### Run PostgreSQL Server with Docker

```shell
docker run --rm -it --pull=always -p 5432:5432 --name steeltoe-postgres -e POSTGRES_DB=steeltoe -e POSTGRES_USER=steeltoe -e POSTGRES_PASSWORD=steeltoe postgres:alpine
```

## RabbitMQ

### Run RabbitMQ Server with Docker

> [!NOTE]
> This image has the management plugin enabled and no credentials set.

```shell
docker run --rm -it --pull=always -p 5672:5672 -p 15672:15672 --name rabbitmq rabbitmq:3-management
```

## Redis

### Run Redis server with Docker

```shell
docker run --rm -it --pull=always -p 6379:6379 --name redis redis
```

## Valkey

### Run Valkey server with Docker

```shell
docker run --rm -it --pull=always -p 6379:6379 --name valkey valkey/valkey
```

## SQL Server

### Run SQL Server with Docker

```shell
docker run --rm -it --pull=always -p 1433:1433 --name mssql -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=St33ltoeR0cks!' mcr.microsoft.com/mssql/server
```

## UAA Server for Steeltoe Samples

The Steeltoe team has created a [UAA configuration](https://github.com/SteeltoeOSS/Dockerfiles/blob/main/uaa-server/uaa.yml) to use with the sample applications in this repository.

### Run UAA Server with Docker

```shell
docker run --rm -it --pull=always -p 8080:8080 --name steeltoe-uaa steeltoe.azurecr.io/uaa-server:latest
```

### Run Steeltoe UAA on Cloud Foundry

Refer to the [README in the Dockerfiles repository](https://github.com/SteeltoeOSS/Dockerfiles/blob/main/uaa-server/README.md) for instructions.

## Zipkin

### Run Zipkin Server with Docker

```shell
docker run --rm -it --pull=always -p 9411:9411 --name zipkin openzipkin/zipkin
```
