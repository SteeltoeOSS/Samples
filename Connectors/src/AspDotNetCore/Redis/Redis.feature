@cloudfoundry
Feature: Redis Connector
  In order to show you how to use Steeltoe for connecting to Redis
  You can run some Redis connection samples

  @netcoreapp3.1
  @win10-x64
  Scenario: Redis Connector for netcoreapp3.1/win10-x64
    When you run: dotnet restore --configfile ../../../../nuget.config
    And you run: dotnet publish -f netcoreapp3.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish
    And you wait until CloudFoundry app redis-connector is started
    When you get https://redis-connector.x.y.z/Home/CacheData
    Then you should see "Key1=Key1Value"
    And you should see "Key2=Key2Value"

  @netcoreapp3.1
  @ubuntu.16.04-x64
  Scenario: Redis Connector for netcoreapp3.1/ubuntu.16.04-x64
    When you run: dotnet restore --configfile ../../../../nuget.config
    And you run: dotnet publish -f netcoreapp3.1 -r ubuntu.16.04-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/ubuntu.16.04-x64/publish
    And you wait until CloudFoundry app redis-connector is started
    When you get https://redis-connector.x.y.z/Home/CacheData
    Then you should see "Key1=Key1Value"
    And you should see "Key2=Key2Value"
