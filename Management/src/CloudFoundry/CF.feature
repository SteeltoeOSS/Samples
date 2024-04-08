@cloudfoundry_scaffold
Feature: Cloud Foundry Samples
  In order to show you how to use Steeltoe Management Endpoint
  You can run some Steeltoe Management Endpoint samples

  @net6.0
  @win10
  Scenario: CloudFoundry Management (net8.0/win10)
    When you run: dotnet publish -r win-x64 --self-contained
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net8.0/win-x64/publish
    And you wait until CloudFoundry app actuator is started
    Then you should be able to access CloudFoundry app actuator management endpoints

  @net6.0
  @linux
  Scenario: CloudFoundry Management (net8.0/linux)
    When you run in the background: cf push -f manifest.yml
    And you wait until CloudFoundry app actuator is started
    Then you should be able to access CloudFoundry app actuator management endpoints
