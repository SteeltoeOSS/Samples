@cloudfoundry_scaffold
Feature: CloudFoundry Single SignOn
  In order to show you how to use Steeltoe with CloudFoundry Single SignOn
  You can run some CloudFoundry Single SignOn samples

  @netcoreapp3.1
  @win10-x64
  Scenario: CloudFoundry Single SignOn (netcoreapp3.1/win10-x64)
    When you run: dotnet publish -f netcoreapp3.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish
    And you wait until CloudFoundry app single-signon is started
    When you get https://single-signon.x.y.z/Home/About
    Then you should be at https://uaa.x.y.z/login
    When you login with "baduser"/"badpass"
    Then you should be at https://uaa.x.y.z/login
    And you should see "Unable to verify email or password. Please try again."
    When you login with "testuser"/"Password1!"
    Then you should be at https://single-signon.x.y.z/Home/About
    And you should see "Your About page."

  @netcoreapp3.1
  @linux-x64
  Scenario: CloudFoundry Single SignOn (netcoreapp3.1/linux-x64)
    When you run: dotnet publish -f netcoreapp3.1 -r linux-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/linux-x64/publish
    And you wait until CloudFoundry app single-signon is started
    When you get https://single-signon.x.y.z/Home/About
    Then you should be at https://uaa.x.y.z/login
    When you login with "baduser"/"badpass"
    Then you should be at https://uaa.x.y.z/login
    And you should see "Unable to verify email or password. Please try again."
    When you login with "testuser"/"Password1!"
    Then you should be at https://single-signon.x.y.z/Home/About
    And you should see "Your About page."

  @net5.0
  @win10-x64
  Scenario: CloudFoundry Single SignOn (net5.0/win10-x64)
    When you run: dotnet publish -f net5.0 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net5.0/win10-x64/publish
    And you wait until CloudFoundry app single-signon is started
    When you get https://single-signon.x.y.z/Home/About
    Then you should be at https://uaa.x.y.z/login
    When you login with "baduser"/"badpass"
    Then you should be at https://uaa.x.y.z/login
    And you should see "Unable to verify email or password. Please try again."
    When you login with "testuser"/"Password1!"
    Then you should be at https://single-signon.x.y.z/Home/About
    And you should see "Your About page."

  @net5.0
  @linux-x64
  Scenario: CloudFoundry Single SignOn (net5.0/linux-x64)
    When you run: dotnet publish -f net5.0 -r linux-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/net5.0/linux-x64/publish
    And you wait until CloudFoundry app single-signon is started
    When you get https://single-signon.x.y.z/Home/About
    Then you should be at https://uaa.x.y.z/login
    When you login with "baduser"/"badpass"
    Then you should be at https://uaa.x.y.z/login
    And you should see "Unable to verify email or password. Please try again."
    When you login with "testuser"/"Password1!"
    Then you should be at https://single-signon.x.y.z/Home/About
    And you should see "Your About page."
