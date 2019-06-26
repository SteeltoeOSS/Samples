#!/usr/bin/env bash

cf delete-service myConfigServer
cf delete-service myDiscoveryService 
cf delete-service myMySqlService
cf delete-service myRedisService
cf delete-service myHystrixService
cf delete-service myOAuthService