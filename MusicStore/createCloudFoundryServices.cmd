@ECHO OFF

SET db_service=p.mysql
SET db_plan=db-small
IF NOT "%1"=="" set db_service=%1
IF NOT "%2"=="" set db_plan=%2
cf create-service p-config-server standard mStoreConfig -c config-server.json
cf create-service p-service-registry standard mStoreRegistry
cf create-service p-circuit-breaker-dashboard standard mStoreHystrix 
cf create-service %db_service% %db_plan% mStoreAccountsDB
cf create-service %db_service% %db_plan% mStoreOrdersDB
cf create-service %db_service% %db_plan% mStoreCartDB
cf create-service %db_service% %db_plan% mStoreStoreDB
IF NOT "%USE_REDIS_CACHE%"=="" (CMD /c "cf create-service p-redis shared-vm mStoreRedis")