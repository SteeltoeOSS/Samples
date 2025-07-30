@cloudfoundry_scaffold
Feature: Security
  In order to show you how to use Steeltoe for using protected shared session state with Redis
  You can run some samples

  @net8.0
  @linux
  Scenario: RedisDataProtection (net8.0/linux)
    When you push: manifest.yml
    And you wait until CloudFoundry app redis-data-protection-sample is started
    When you get https://redis-data-protection-sample/
    Then you should see "Example Protected Text"
