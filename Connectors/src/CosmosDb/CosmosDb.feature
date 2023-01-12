@cloudfoundry_scaffold
Feature: CosmosDb Connector
  In order to show you how to use Steeltoe for connecting to CosmosDb
  You can run some CosmosDb connection samples

  @net7.0
  @windows
  Scenario: CosmosDb Connector (net7.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net7.0/win-x64/publish
    And you wait until CloudFoundry app cosmosdb-connector is started
    When you get https://cosmosdb-connector/
    Then you should see "Object1"
    And you should see "Object2"

  @net7.0
  @linux
  Scenario: CosmosDb Connector (net7.0/linux)
    When you run in the background: cf push -f manifest.yml
    And you wait until CloudFoundry app cosmosdb-connector is started
    When you get https://cosmosdb-connector/
    Then you should see "Object1"
    And you should see "Object2"
