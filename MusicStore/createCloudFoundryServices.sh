#!/usr/bin/env bash
set -ex

db_service=p.mysql
db_plan=db-small


if  [ "$1" != "" ]; then 
	db_service=$1
fi

if  [ "$2" != "" ]; then 
	db_plan=$2
fi

cf create-service p-circuit-breaker-dashboard standard mStoreHystrix 
cf create-service p-config-server standard mStoreConfig -c config-server.json
cf create-service p-service-registry standard mStoreRegistry 
cf create-service "$db_service" "$db_plan" mStoreAccountsDB
cf create-service "$db_service" "$db_plan" mStoreOrdersDB
cf create-service "$db_service" "$db_plan" mStoreCartDB
cf create-service "$db_service" "$db_plan" mStoreStoreDB


svc_count=6
if [ "$USE_REDIS_CACHE" != "" ]; then
	cf create-service p-redis shared-vm mStoreRedis
	svc_count=7
fi

set +x
while [ `cf services | grep 'in progress' | wc -l | sed 's/ //g'` != 0 ]; do
  echo 'Waiting for services to start'
  cf services | grep 'in progress'
  echo
  sleep 5
done

if [ `cf services | grep 'create succeeded' | wc -l | sed 's/ //g'` != $svc_count ]; then
  echo 'Not all services were successfully created'
  cf services
  echo
  exit 1
fi

echo "All $svc_count services were successfully created"
