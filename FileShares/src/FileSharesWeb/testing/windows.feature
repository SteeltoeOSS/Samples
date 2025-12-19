@cloudfoundry_scaffold
Feature: FileShares
  In order to show you how to use Steeltoe for connecting to Windows network file shares
  You can run the network file share sample

  @net10.0
  @windows
  Scenario: FileSharesWeb (net10.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you push: manifest-windows.yml with args: -p bin/Release/net10.0/win-x64/publish
    And you wait until CloudFoundry app fileshares-sample is started
    Then CloudFoundry app fileshares-sample should be healthy
