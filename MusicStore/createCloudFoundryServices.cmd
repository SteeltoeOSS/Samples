cf create-service p-config-server standard mStoreConfig -c config-server.json
cf create-service p-service-registry standard mStoreRegistry 
cf create-service p-mysql pre-existing-plan mStoreAccountsDB
cf create-service p-mysql pre-existing-plan mStoreOrdersDB
cf create-service p-mysql pre-existing-plan mStoreCartDB
cf create-service p-mysql pre-existing-plan mStoreStoreDB