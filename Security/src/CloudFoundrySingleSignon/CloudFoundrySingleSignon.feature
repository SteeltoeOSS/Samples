@cloudfoundry_scaffold
Feature: CloudFoundry Single SignOn
  In order to show you how to use Steeltoe with CloudFoundry Single SignOn
  You can run some CloudFoundry Single SignOn samples

  @net6.0
  @win10
  Scenario: CloudFoundry Single SignOn (net6.0/win10)
    When you run: dotnet publish -f net6.0 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net6.0/win10-x64/publish
    And you wait until CloudFoundry app single-signon is started
    When you get https://single-signon.x.y.z/Home/About
    Then you should be at https://uaa.x.y.z/login
    When you login with "baduser"/"badpass"
    Then you should be at https://uaa.x.y.z/login
    And you should see "Unable to verify email or password. Please try again."
    When you login with "testuser"/"Password1!"
    Then you should be at https://single-signon.x.y.z/Home/About
    And you should see "Your About page."

  @net6.0
  @linux
  Scenario: CloudFoundry Single SignOn (net6.0/linux)
    When you run in the background: cf push -f manifest.yml
    And you wait until CloudFoundry app single-signon is started
    When you get https://single-signon.x.y.z/Home/About
    Then you should be at https://uaa.x.y.z/login
    When you login with "baduser"/"badpass"
    Then you should be at https://uaa.x.y.z/login
    And you should see "Unable to verify email or password. Please try again."
    When you login with "testuser"/"Password1!"
    Then you should be at https://single-signon.x.y.z/Home/About
    And you should see "Your About page."
