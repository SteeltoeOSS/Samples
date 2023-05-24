@cloudfoundry_scaffold
Feature: OAuth Connector
  In order to show you how to use Steeltoe for connecting to OAuth
  You can run some OAuth connection samples

  @net6.0
  @windows
  Scenario: OAuth Connector (net6.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you run: cf push -f manifest-windows.yml -p bin/Debug/net6.0/win-x64/publish
    And you wait until CloudFoundry app oauth-connector is started
    When you get https://oauth-connector/
    Then you should see "a, b, c, d"

  @net6.0
  @linux
  Scenario: OAuth Connector (net6.0/linux)
    When you run: cf push -f manifest.yml
    And you wait until CloudFoundry app oauth-connector is started
    When you get https://oauth-connector/
    Then you should see "a, b, c, d"
