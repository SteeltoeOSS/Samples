@cloudfoundry_scaffold
Feature: Cloud Foundry Samples
  In order to show you how to use Steeltoe Management Endpoint
  You can run some Steeltoe Management Endpoint samples

  @net6.0
  @win10-x64
  Scenario: CloudFoundry Management (net6.0/win10-x64)
    When you run: dotnet publish -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net6.0/win10-x64/publish
    And you wait until CloudFoundry app actuator is started
    Then you should be able to access CloudFoundry app actuator management endpoints

  @net6.0
  @linux-x64
  Scenario: CloudFoundry Management (net6.0/linux-x64)
    When you run in the background: cf push -f manifest.yml
    And you wait until CloudFoundry app actuator is started
    Then you should be able to access CloudFoundry app actuator management endpoints
