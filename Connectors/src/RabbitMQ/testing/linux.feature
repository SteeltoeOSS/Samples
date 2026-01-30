@cloudfoundry_scaffold
Feature: Connectors
  In order to show you how to use Steeltoe for connecting to RabbitMQ
  You can run some RabbitMQ connection samples

  @net10.0
  @linux
  Scenario: RabbitMQ (net10.0/linux)
    When you push: manifest.yml
    And you wait until CloudFoundry app rabbitmq-connector-sample is started
    When you post "MessageToSend=HEY THERE" to https://rabbitmq-connector-sample/Home/Send
    And you get https://rabbitmq-connector-sample/Home/Receive
    Then you should see "HEY THERE"
