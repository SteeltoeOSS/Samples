@cloudfoundry_scaffold
Feature: Management
  In order to show you how to use Steeltoe Actuators
  You can run some Steeltoe Management samples

  @net8.0
  @linux
  Scenario: ActuatorApi (net8.0/linux)
    When you push: manifest.yml with args: --random-route
    And you wait until CloudFoundry app actuator-api-management-sample is started
    Then you should be able to access CloudFoundry app actuator-api-management-sample management endpoints
    When you run: cf run-task actuator-api-management-sample --command "./bin/Debug/net8.0/linux-x64/Steeltoe.Samples.ActuatorApi runtask=MigrateDatabase --name MigrateDatabase"
    And you wait until CloudFoundry task MigrateDatabase for actuator-api-management-sample is successful
    When you run: cf run-task actuator-api-management-sample --command "./bin/Debug/net8.0/linux-x64/Steeltoe.Samples.ActuatorApi runtask=ForecastWeather --name ForecastWeather"
    And you wait until CloudFoundry task ForecastWeather for actuator-api-management-sample is successful
    When you get https://actuator-api-management-sample/weatherforecast
    Then the response should contain "temperatureF"
