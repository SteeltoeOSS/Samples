#!/usr/bin/env bash

cf create-service p-config-server standard myConfigServer -c config-server.json
cf create-service p-service-registry standard myDiscoveryService 
cf create-service p-mysql 100mb myMySqlService
cf create-service p-redis shared-vm myRedisService
cf create-service p-circuit-breaker-dashboard standard myHystrixService
cf cups myOAuthService -p `{"client_id": "myTestApp","client_secret": "myTestApp","uri": "uaa://login.system.testcloud.com"}'