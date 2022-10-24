@local_scaffold
Feature: Simple Configuration
  In order to show you how to use Steeltoe for simple configurations
  You can run a some simple configuration samples

  @net6.0
  Scenario: Simple Configuration (net6.0)
    When you run: git clone https://github.com/spring-cloud/spring-cloud-config
    And you run: git -C spring-cloud-config checkout v2.1.4.RELEASE
    And you run in the background: mvn -f spring-cloud-config/spring-cloud-config-server/pom.xml spring-boot:run
    And you wait until process listening on port 8888
    And you set env var ASPNETCORE_ENVIRONMENT to "MyNetCoreEnv"
    And you run in the background: dotnet run
    And you wait until process listening on port 5000
    When you get http://localhost:5000/Home/ConfigServerSettings
    Then you should see "spring:cloud:config:name = foo"
    And you should see "spring:cloud:config:env = MyNetCoreEnv"
