@cloudfoundry_scaffold
Feature: Simple CloudFoundry Configuration
  In order to show you how to use Steeltoe for simple Cloud Foundry configurations
  You can run some simple CloudFoundry configuration samples

  @netcoreapp3.1
  @win10-x64
  Scenario: Simple CloudFoundry Configuration (netcoreapp3.1/win10-x64)
    When you run: dotnet publish -f netcoreapp3.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish
    And you wait until CloudFoundry app foo is started
    When you get https://foo.x.y.z/Home/ConfigServerSettings
    Then you should see "spring:cloud:config:name = foo"

  @net5.0
  @win10-x64
  Scenario: Simple CloudFoundry Configuration (net5.0/win10-x64)
    When you run: dotnet publish -f net5.0 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net5.0/win10-x64/publish
    And you wait until CloudFoundry app foo is started
    When you get https://foo.x.y.z/Home/ConfigServerSettings
    Then you should see "spring:cloud:config:name = foo"

  @netcoreapp3.1
  @linux-x64
  Scenario: Simple CloudFoundry Configuration (netcoreapp3.1/linux-x64)
    When you run: dotnet publish -f netcoreapp3.1 -r linux-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/linux-x64/publish
    And you wait until CloudFoundry app foo is started
    When you get https://foo.x.y.z/Home/ConfigServerSettings
    Then you should see "spring:cloud:config:name = foo"

  @net5.0
  @linux-x64
  Scenario: Simple CloudFoundry Configuration (net5.0/linux-x64)
    When you run: dotnet publish -f net5.0 -r linux-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/net5.0/linux-x64/publish
    And you wait until CloudFoundry app foo is started
    When you get https://foo.x.y.z/Home/ConfigServerSettings
    Then you should see "spring:cloud:config:name = foo"
