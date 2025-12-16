@cloudfoundry_scaffold
Feature: Connectors
  In order to show you how to use Steeltoe for connecting to Redis
  You can run some Redis connection samples

  @net10.0
  @windows
  Scenario: Redis (net10.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you push: manifest-windows.yml with args: -p bin/Release/net10.0/win-x64/publish
    And you wait until CloudFoundry app redis-connector-sample is started
    When you get https://redis-connector-sample/
    Then you should see "Hello from Lua"
    And you should see "redis-connector-sampleKeySetUsingMicrosoftApi1"
    And you should see "ValueSetUsingMicrosoftApi1"
    And you should see "redis-connector-sampleKeySetUsingMicrosoftApi2"
    And you should see "ValueSetUsingMicrosoftApi2"
    And you should see "redis-connector-sampleKeySetUsingRedisApi1"
    And you should see "ValueSetUsingRedisApi1"
    And you should see "redis-connector-sampleKeySetUsingRedisApi2"
    And you should see "ValueSetUsingRedisApi2"
