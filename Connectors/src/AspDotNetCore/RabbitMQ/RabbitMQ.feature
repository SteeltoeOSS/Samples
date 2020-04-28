@cloudfoundry
Feature: RabbitMQ Connector
  In order to show you how to use Steeltoe for connecting to RabbitMQ
  You can run some RabbitMQ connection samples

  @netcoreapp3.1
  @win10-x64
  Scenario: Rabbit Connector for netcoreapp3.1/win10-x64
    Given your Cloud Foundry services have been deployed
    When you run: dotnet publish -f netcoreapp3.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish
    And you wait until CloudFoundry app rabbitmq-connector is started
    When you post "Message=HEY THERE 1" to https://rabbitmq-connector.x.y.z/RabbitMQ/Send
    And you get https://rabbitmq-connector.x.y.z/RabbitMQ/Receive
    Then you should see "Message=HEY THERE 1"

  @netcoreapp3.1
  @ubuntu.16.04-x64
  Scenario: Rabbit Connector for netcoreapp3.1/ubuntu.16.04-x64
    Given your Cloud Foundry services have been deployed
    When you run: dotnet publish -f netcoreapp3.1 -r ubuntu.16.04-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/ubuntu.16.04-x64/publish
    And you wait until CloudFoundry app rabbitmq-connector is started
    When you post "Message=HEY THERE 2" to https://rabbitmq-connector.x.y.z/RabbitMQ/Send
    And you get https://rabbitmq-connector.x.y.z/RabbitMQ/Receive
    Then you should see "Message=HEY THERE 2"

  @netcoreapp2.1
  @win10-x64
  Scenario: Rabbit Connector for netcoreapp2.1/win10-x64
    Given your Cloud Foundry services have been deployed
    When you run: dotnet publish -f netcoreapp2.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish
    And you wait until CloudFoundry app rabbitmq-connector is started
    When you post "Message=HEY THERE 3" to https://rabbitmq-connector.x.y.z/RabbitMQ/Send
    And you get https://rabbitmq-connector.x.y.z/RabbitMQ/Receive
    Then you should see "Message=HEY THERE 3"

  @netcoreapp2.1
  @ubuntu.16.04-x64
  Scenario: Rabbit Connector for netcoreapp2.1/ubuntu.16.04-x64
    Given your Cloud Foundry services have been deployed
    When you run: dotnet publish -f netcoreapp2.1 -r ubuntu.16.04-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.16.04-x64/publish
    And you wait until CloudFoundry app rabbitmq-connector is started
    When you post "Message=HEY THERE 4" to https://rabbitmq-connector.x.y.z/RabbitMQ/Send
    And you get https://rabbitmq-connector.x.y.z/RabbitMQ/Receive
    Then you should see "Message=HEY THERE 4"

  @net461
  @win10-x64
  Scenario: Rabbit Connector for net461/win10-x64
    Given your Cloud Foundry services have been deployed
    When you run: dotnet publish -f net461 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish
    And you wait until CloudFoundry app rabbitmq-connector is started
    When you post "Message=HEY THERE 5" to https://rabbitmq-connector.x.y.z/RabbitMQ/Send
    And you get https://rabbitmq-connector.x.y.z/RabbitMQ/Receive
    Then you should see "Message=HEY THERE 5"
