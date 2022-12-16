@cloudfoundry_scaffold
Feature: Redis Connector
  In order to show you how to use Steeltoe for connecting to Redis
  You can run some Redis connection samples

  @net6.0
  @win10
  Scenario: Redis Connector (net6.0/win10)
    When you run: dotnet publish -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net6.0/win10-x64/publish
    And you wait until CloudFoundry app redis-connector is started
    When you get https://redis-connector/Home/CacheData
    Then you should see "Key1=Key1Value"
    And you should see "Key2=Key2Value"

  @net6.0
  @linux
  Scenario: Redis Connector (net6.0/linux)
    When you run in the background: cf push -f manifest.yml
    And you wait until CloudFoundry app redis-connector is started
    When you get https://redis-connector/Home/CacheData
    Then you should see "Key1=Key1Value"
    And you should see "Key2=Key2Value"
