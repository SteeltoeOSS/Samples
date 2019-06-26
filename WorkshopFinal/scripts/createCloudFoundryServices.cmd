@ECHO OFF

cf create-service p-config-server standard myConfigServer -c config-server.json
cf create-service p-service-registry standard myDiscoveryService 
cf create-service p-mysql 100mb myMySqlService
cf create-service p-redis shared-vm myRedisService
cf create-service p-circuit-breaker-dashboard standard myHystrixService
cf cups myOAuthService -p "{\"client_id\": \"dave1App\",\"client_secret\": \"dave1Secret\",\"uri\": \"uaa://login.sys.selma.cf-app.com\"}"
