@#152883475
@local
Feature: Simple Configuration
    In order to show you how to use Steeltoe for simple configurations
    You can run a some simple configuration samples

    @netcoreapp2.1
    Scenario Outline: Simple Configuration for .Net Core 2.1
        Given you have at least .NET Core SDK 2.1.300 installed
        And you have Java 8 installed
        And you have Apache Maven 3 installed
        When you run: git clone https://github.com/spring-cloud/spring-cloud-config
        And you run: git -C spring-cloud-config checkout v1.3.3.RELEASE
        And you run in the background: mvn -f spring-cloud-config/spring-cloud-config-server/pom.xml spring-boot:run
        And you wait until process listening on port 8888
        And you run: dotnet restore
        And you set env var <env_name> to "<env_value>"
        And you run in the background: dotnet run -f netcoreapp2.1
        And you wait until process listening on port 5000
        When you get http://localhost:5000/Home/ConfigServerSettings
        Then you should see "spring:cloud:config:name = foo"
        And you should see "spring:cloud:config:env = <env_text>"

        Examples:
            | env_name               | env_value   | env_text    |
            | ASPNETCORE_ENVIRONMENT |             | Production  |
            | ASPNETCORE_ENVIRONMENT | Development | Development |

    @net461
    Scenario Outline: Simple Configuration for .Net Framework 4.6.1
        Given you have at least .NET Core SDK 2.1.300 installed
        And you have Java 8 installed
        And you have Apache Maven 3 installed
        When you run: git clone https://github.com/spring-cloud/spring-cloud-config
        And you run: git -C spring-cloud-config checkout v1.3.3.RELEASE
        And you run in the background: mvn -f spring-cloud-config/spring-cloud-config-server/pom.xml spring-boot:run
        And you wait until process listening on port 8888
        And you run: dotnet restore
        And you set env var <env_name> to "<env_value>"
        And you run in the background: dotnet run -f net461
        And you wait until process listening on port 5000
        When you get http://localhost:5000/Home/ConfigServerSettings
        Then you should see "spring:cloud:config:name = foo"
        And you should see "spring:cloud:config:env = <env_text>"

        Examples:
            | env_name               | env_value   | env_text    |
            | ASPNETCORE_ENVIRONMENT |             | Production  |
            | ASPNETCORE_ENVIRONMENT | Development | Development |
