@cloudfoundry_scaffold
Feature: Security
  In order to show you how to use Steeltoe for using protected shared session state with Redis
  You can run some samples

  @net8.0
  @windows
  Scenario: RedisDataProtection (net8.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you push: manifest-windows.yml with args: -p bin/Release/net8.0/win-x64/publish
    And you wait until CloudFoundry app redis-data-protection-sample is started
    When you get https://redis-data-protection-sample/
    Then you should see "Example Protected Text"
