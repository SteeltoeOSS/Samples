#!/usr/bin/env bash
set -eux

cf create-service p-config-server standard mStoreConfig -c config-server.json
cf create-service p-service-registry standard mStoreRegistry 
cf create-service p-mysql pre-existing-plan mStoreAccountsDB
cf create-service p-mysql pre-existing-plan mStoreOrdersDB
cf create-service p-mysql pre-existing-plan mStoreCartDB
cf create-service p-mysql pre-existing-plan mStoreStoreDB

set +x
while [ `cf services | grep 'in progress' | wc -l | sed 's/ //g'` != 0 ]; do
  echo 'Waiting for services to start'
  cf services | grep 'in progress'
  echo
  sleep 5
done

if [ `cf services | grep 'create succeeded' | wc -l | sed 's/ //g'` != 6 ]; then
  echo 'Not all services were successfully created'
  cf services
  echo
  exit 1
fi

echo 'All 6 services were successfully created'
