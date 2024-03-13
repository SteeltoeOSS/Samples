@cloudfoundry_scaffold
Feature: CloudFoundry Configuration
  In order to show you how to use Steeltoe for Cloud Foundry configurations
  You can run some CloudFoundry configuration samples

  @net6.0
  @win10
  Scenario: CloudFoundry Configuration (net6.0/win10)
    When you run: dotnet publish -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net6.0/win10-x64/publish
    And you wait until CloudFoundry app cloud is started
    When you get https://cloud/Home/CloudFoundry
    Then you should see "vcap:application:application_name = cloud"

  @net6.0
  @linux
  Scenario: CloudFoundry Configuration (net6.0/linux)
    When you run: dotnet build -r linux-x64
    And you run in the background: cf push -f manifest.yml
    And you wait until CloudFoundry app cloud is started
    When you get https://cloud/Home/CloudFoundry
    Then you should see "vcap:application:application_name = cloud"
