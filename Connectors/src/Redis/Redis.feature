@cloudfoundry_scaffold
Feature: Redis Connector
  In order to show you how to use Steeltoe for connecting to Redis
  You can run some Redis connection samples

  @net6.0
  @windows
  Scenario: Redis Connector (net6.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you run: cf push -f manifest-windows.yml -p bin/Debug/net6.0/win-x64/publish
    And you wait until CloudFoundry app redis-connector is started
    When you get https://redis-connector/
    Then you should see "redis-connectorKeySetUsingMicrosoftApi1"
    Then you should see "ValueSetUsingMicrosoftApi1"
    Then you should see "redis-connectorKeySetUsingMicrosoftApi2"
    Then you should see "ValueSetUsingMicrosoftApi2"
    Then you should see "redis-connectorKeySetUsingRedisApi1"
    Then you should see "ValueSetUsingRedisApi1"
    Then you should see "redis-connectorKeySetUsingRedisApi2"
    Then you should see "ValueSetUsingRedisApi2"
    And you should see "Hello from Lua"

  @net6.0
  @linux
  Scenario: Redis Connector (net6.0/linux)
    When you run: cf push -f manifest.yml
    And you wait until CloudFoundry app redis-connector is started
    When you get https://redis-connector/
    Then you should see "redis-connectorKeySetUsingMicrosoftApi1"
    Then you should see "ValueSetUsingMicrosoftApi1"
    Then you should see "redis-connectorKeySetUsingMicrosoftApi2"
    Then you should see "ValueSetUsingMicrosoftApi2"
    Then you should see "redis-connectorKeySetUsingRedisApi1"
    Then you should see "ValueSetUsingRedisApi1"
    Then you should see "redis-connectorKeySetUsingRedisApi2"
    Then you should see "ValueSetUsingRedisApi2"
    And you should see "Hello from Lua"
