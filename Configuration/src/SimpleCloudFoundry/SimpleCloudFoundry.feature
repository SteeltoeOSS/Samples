@cloudfoundry_scaffold
Feature: Simple CloudFoundry Configuration
  In order to show you how to use Steeltoe for simple Cloud Foundry configurations
  You can run some simple CloudFoundry configuration samples

  @net6.0
  @win10-x64
  Scenario: Simple CloudFoundry Configuration (net6.0/win10-x64)
    When you run: dotnet publish -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net6.0/win10-x64/publish
    And you wait until CloudFoundry app foo is started
    When you get https://foo.x.y.z/Home/ConfigServerSettings
    Then you should see "spring:cloud:config:name = foo"

  @net6.0
  @linux-x64
  Scenario: Simple CloudFoundry Configuration (net6.0/linux-x64)
    When you run: dotnet build
    And you run in the background: cf push -f manifest.yml
    And you wait until CloudFoundry app foo is started
    When you get https://foo.x.y.z/Home/ConfigServerSettings
    Then you should see "spring:cloud:config:name = foo"
