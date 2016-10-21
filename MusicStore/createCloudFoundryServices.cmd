@ECHO OFF

cf create-service p-config-server standard mStoreConfig -c config-server.json
cf create-service p-service-registry standard mStoreRegistry 
cf create-service p-mysql 100mb mStoreAccountsDB
cf create-service p-mysql 100mb mStoreOrdersDB
cf create-service p-mysql 100mb mStoreCartDB
cf create-service p-mysql 100mb mStoreStoreDB
IF NOT "%USE_REDIS_CACHE%"=="" (CMD /c "cf create-service p-redis shared-vm mStoreRedis")