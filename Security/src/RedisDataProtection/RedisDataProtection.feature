@cloudfoundry_scaffold
Feature: Redis Data Protection Sample
  In order to show you how to use Steeltoe for using protected shared session state with Redis
  You can run some samples

  @net8.0
  @windows
  Scenario: Redis Data Protection Sample (net8.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you run: cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
    And you wait until CloudFoundry app redis-data-protection-sample is started
    When you get https://redis-data-protection-sample/
    Then you should see "Example Protected Text"

  @net8.0
  @linux
  Scenario: Redis Data Protection Sample (net8.0/linux)
    When you run: cf push -f manifest.yml
    And you wait until CloudFoundry app redis-data-protection-sample is started
    When you get https://redis-data-protection-sample/
    Then you should see "Example Protected Text"
