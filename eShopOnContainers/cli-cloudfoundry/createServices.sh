#!/bin/sh

cf create-service p-rabbitmq standard eShopMQ
cf create-service p-redis shared-vm eShopCache
cf create-service mongodb-odb standalone eShopDocDb
cf create-service p-mysql 100mb eShopIdentityDb
cf create-service p-mysql 100mb eShopCatalogDb
cf create-service p-mysql 100mb eShopOrderingDb
cf create-service p-mysql 100mb eShopMarketingDb
