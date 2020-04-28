@cloudfoundry
Feature: Simple CloudFoundry Configuration
  In order to show you how to use Steeltoe for simple CloudFoundry configurations
  You can run some simple CloudFoundry configuration samples

  @netcoreapp3.1
  @win10-x64
  Scenario: Simple CloudFoundry Configuration for netcoreapp3.1/win10-x64
    Given your Cloud Foundry services have been deployed
    When you run: dotnet publish -f netcoreapp3.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish
    And you wait until CloudFoundry app foo is started
    When you get https://foo.x.y.z/Home/ConfigServerSettings
    Then you should see "spring:cloud:config:name = foo"

  @netcoreapp3.1
  @ubuntu.16.04-x64
  Scenario: Simple CloudFoundry Configuration for netcoreapp3.1/ubuntu.16.04-x64
    Given your Cloud Foundry services have been deployed
    When you run: dotnet publish -f netcoreapp3.1 -r ubuntu.16.04-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/ubuntu.16.04-x64/publish
    And you wait until CloudFoundry app foo is started
    When you get https://foo.x.y.z/Home/ConfigServerSettings
    Then you should see "spring:cloud:config:name = foo"

  @netcoreapp2.1
  @win10-x64
  Scenario: Simple CloudFoundry Configuration for netcoreapp2.1/win10-x64
    Given your Cloud Foundry services have been deployed
    When you run: dotnet publish -f netcoreapp2.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish
    And you wait until CloudFoundry app foo is started
    When you get https://foo.x.y.z/Home/ConfigServerSettings
    Then you should see "spring:cloud:config:name = foo"

  @netcoreapp2.1
  @ubuntu.16.04-x64
  Scenario: Simple CloudFoundry Configuration for netcoreapp2.1/ubuntu.16.04-x64
    Given your Cloud Foundry services have been deployed
    When you run: dotnet publish -f netcoreapp2.1 -r ubuntu.16.04-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.16.04-x64/publish
    And you wait until CloudFoundry app foo is started
    When you get https://foo.x.y.z/Home/ConfigServerSettings
    Then you should see "spring:cloud:config:name = foo"

  @net461
  @win10-x64
  Scenario: Simple CloudFoundry Configuration for net461/win10-x64
    Given your Cloud Foundry services have been deployed
    When you run: dotnet publish -f net461 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish
    And you wait until CloudFoundry app foo is started
    When you get https://foo.x.y.z/Home/ConfigServerSettings
    Then you should see "spring:cloud:config:name = foo"
