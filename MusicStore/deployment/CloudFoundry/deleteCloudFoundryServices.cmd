@ECHO OFF
cf delete musicstore -r -f
cf delete musicui -r -f
cf delete-service mStoreConfig -f
cf delete-service mStoreRegistry -f
cf delete-service mStoreAccountsDB -f
cf delete-service mStoreOrdersDB -f
cf delete-service mStoreCartDB -f
cf delete-service mStoreStoreDB -f
cf delete-service mStoreRedis -f
cf delete-service mStoreHystrix -f