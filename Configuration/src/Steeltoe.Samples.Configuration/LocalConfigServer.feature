@local_scaffold
Feature: Local Spring Cloud Config Server
  In order to show you how to use Steeltoe for simple configurations
  You can run a simple configuration sample

  @net8.0
  Scenario: Simple Configuration (net8.0)
    When you run: git clone https://github.com/spring-cloud/spring-cloud-config
    And you run: git -C spring-cloud-config checkout v4.1.0
    And you run in the background: ./mvnw -f spring-cloud-config/spring-cloud-config-server/pom.xml spring-boot:run
    And you wait until process listening on port 8888
    And you set env var ASPNETCORE_ENVIRONMENT to "MyNetCoreEnv"
    And you run in the background: dotnet run
    And you wait until process listening on port 7021
    When you get https://localhost:7021/Home/ConfigServerSettings
    Then you should see "spring:cloud:config:name = foo"
    And you should see "spring:cloud:config:env = MyNetCoreEnv"
