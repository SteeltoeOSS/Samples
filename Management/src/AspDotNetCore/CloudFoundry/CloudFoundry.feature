@cloudfoundry_scaffold
Feature: Cloud Foundry Samples
  In order to show you how to use Steeltoe Management Endpoint
  You can run some Steeltoe Management Endpoint samples

  @netcoreapp3.1
  @win10-x64
  Scenario: CloudFoundry Management (netcoreapp3.1/win10-x64)
    When you run: dotnet publish -f netcoreapp3.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish
    And you wait until CloudFoundry app actuator is started
    Then you should be able to access CloudFoundry app actuator management endpoints

  @netcoreapp3.1
  @ubuntu.16.04-x64
  Scenario: CloudFoundry Management (netcoreapp3.1/ubuntu.16.04-x64)
    When you run: dotnet publish -f netcoreapp3.1 -r ubuntu.16.04-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/ubuntu.16.04-x64/publish
    And you wait until CloudFoundry app actuator is started
    Then you should be able to access CloudFoundry app actuator management endpoints

  @netcoreapp2.1
  @win10-x64
  Scenario: CloudFoundry Management (netcoreapp2.1/win10-x64)
    When you run: dotnet publish -f netcoreapp2.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish
    And you wait until CloudFoundry app actuator is started
    Then you should be able to access CloudFoundry app actuator management endpoints

  @netcoreapp2.1
  @ubuntu.16.04-x64
  Scenario: CloudFoundry Management (netcoreapp2.1/ubuntu.16.04-x64)
    When you run: dotnet publish -f netcoreapp2.1 -r ubuntu.16.04-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.16.04-x64/publish
    And you wait until CloudFoundry app actuator is started
    Then you should be able to access CloudFoundry app actuator management endpoints

  @net461
  @win10-x64
  Scenario: CloudFoundry Management (net461/win10-x64)
    When you run: dotnet publish -f net461 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish
    And you wait until CloudFoundry app actuator is started
    Then you should be able to access CloudFoundry app actuator management endpoints
