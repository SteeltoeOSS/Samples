@cloudfoundry_scaffold
Feature: Simple CloudFoundry Configuration
  In order to show you how to use Steeltoe for simple Cloud Foundry configurations
  You can run some simple CloudFoundry configuration samples

  @net8.0
  @windows
  Scenario: Simple CloudFoundry Configuration (net8.0/win)
    When you run: dotnet publish -r win-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net8.0/win-x64/publish
    And you wait until CloudFoundry app foo is started
    When you get https://foo/Home/ConfigServerSettings
    Then you should see "spring:cloud:config:name = foo"

  @net8.0
  @linux
  Scenario: Simple CloudFoundry Configuration (net8.0/linux)
    When you run: dotnet build
    And you run in the background: cf push -f manifest.yml
    And you wait until CloudFoundry app foo is started
    When you get https://foo/Home/ConfigServerSettings
    Then you should see "spring:cloud:config:name = foo"
