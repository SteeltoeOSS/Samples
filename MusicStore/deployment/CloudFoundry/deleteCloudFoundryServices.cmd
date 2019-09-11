@ECHO OFF

cf delete-service mStoreConfig
cf delete-service mStoreRegistry 
cf delete-service mStoreAccountsDB
cf delete-service mStoreOrdersDB
cf delete-service mStoreCartDB
cf delete-service mStoreStoreDB
cf delete-service mStoreRedis
cf delete-service mStoreHystrix