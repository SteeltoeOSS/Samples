@cloudfoundry_scaffold
Feature: Redis Connector
  In order to show you how to use Steeltoe for connecting to Redis
  You can run some Redis connection samples

  @net7.0
  @windows
  Scenario: Redis Connector (net7.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net7.0/win-x64/publish
    And you wait until CloudFoundry app redis-connector is started
    When you get https://redis-connector/
    Then you should see "CacheValue1"
    And you should see "CacheValue2"
    And you should see "ConnectionMultiplexerValue1"
    And you should see "ConnectionMultiplexerValue2"
    And you should see "Hello from Lua"

  @net7.0
  @linux
  Scenario: Redis Connector (net7.0/linux)
    When you run in the background: cf push -f manifest.yml
    And you wait until CloudFoundry app redis-connector is started
    When you get https://redis-connector/
    Then you should see "CacheValue1"
    And you should see "CacheValue2"
    And you should see "ConnectionMultiplexerValue1"
    And you should see "ConnectionMultiplexerValue2"
    And you should see "Hello from Lua"
