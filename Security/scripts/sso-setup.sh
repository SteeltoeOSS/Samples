cf create-service smb Existing SERVICE-INSTANCE-NAME -c '{"share":"//SERVER/SHARE"}'





#!/bin/bash
set -ex
# vars to define
SYSTEM_DOMAIN=systemdomain.com
# UAA_ENDPOINT eg uaa.systemdomain.com
UAA_ENDPOINT=uaa.$SYSTEM_DOMAIN
# Get credentials from the runtime tile (eg: Tanzu Application Service) in Ops Manager, under UAA -> Admin Client
ADMIN_CLIENT_ID=admin
# ADMIN_CLIENT_SECRET get this from ops manager
ADMIN_CLIENT_SECRET=LhCN75-pYpm-7M0HFVdSwLKDGnLPxUk1

ADMIN_CLIENT_SECRET=LRJiMSrHbg06NmTm7vG_F_50YoZB0v1x

#638c87a2-e620-4d02-ac8d-9d18cef7ab00
# IDENTITY_ZONE_ID this is the guid of the identity zone, it’s the first guid in the URI for any page in the Tanzu SSO UI 
IDENTITY_ZONE_ID=638c87a2-e620-4d02-ac8d-9d18cef7ab00
# ZONEADMIN_CLIENT_ID set an id for the zone-specific admin client we're going to create to be able to create groups and users
ZONEADMIN_CLIENT_ID=adminclient2
# ZONEADMIN_CLIENT_SECRET set your own password for the zone client
ZONEADMIN_CLIENT_SECRET=adminsecret2
# ZONE_ENDPOINT eg auth.login.systemdomain.com
ZONE_ENDPOINT=https://auth.login.$SYSTEM_DOMAIN

SSO_USER=dave
SSO_USER_PASSWORD=Password1!
SSO_USER_EMAIL=dave@testcloud.com
SSO_USER_FN=Dave
SSO_USER_LN=Tillman

uaac target $UAA_ENDPOINT --skip-ssl-validation
uaac token client get $ADMIN_CLIENT_ID -s $ADMIN_CLIENT_SECRET
uaac client add temp --authorities zones.write,scim.zones --scope zones.$IDENTITY_ZONE_ID.admin --authorized_grant_types client_credentials,password -s $ZONEADMIN_CLIENT_SECRET
uaac user add temp --email cf-spring-cloud-services@pivotal.io -p $ZONEADMIN_CLIENT_SECRET

uaac member add zones.$IDENTITY_ZONE_ID.admin temp
uaac token owner get temp temp -p $ZONEADMIN_CLIENT_SECRET -s $ZONEADMIN_CLIENT_SECRET
uaac curl /oauth/clients -k -H "Content-type:application/json" -H "X-Identity-Zone-Id:$IDENTITY_ZONE_ID" -X POST -d "{\"client_id\":\"$ZONEADMIN_CLIENT_ID\",\"client_secret\":\"$ZONEADMIN_CLIENT_SECRET\",\"scope\":[\"uaa.none\"],\"resource_ids\":[\"none\"],\"authorities\":[\"uaa.admin\",\"clients.read\",\"clients.write\",\"scim.read\",\"scim.write\",\"clients.secret\"],\"authorized_grant_types\":[\"client_credentials\"]}"
uaac token client get $ADMIN_CLIENT_ID -s $ADMIN_CLIENT_SECRET
uaac user delete temp
uaac client delete temp

uaac target $ZONE_ENDPOINT --skip-ssl-validation
uaac token client get $ZONEADMIN_CLIENT_ID -s $ZONEADMIN_CLIENT_SECRET
uaac user add $SSO_USER --email $SSO_USER_EMAIL --given_name $SSO_USER_FN --family_name $SSO_USER_LN -p $SSO_USER_PASSWORD
uaac group add testgroup
uaac member add testgroup $SSO_USER








uaac curl 'https://auth.login.sys.pasorobles.cf-app.com/identity-providers/c509d9be-68c1-49aa-8855-0f3064049ce3?rawConfig=true' -X PUT \
  -H 'Content-Type: application/json' \
  -d '{
    "type": "uaa",
    "config": "{\"emailDomain\":null,\"additionalConfiguration\":null,\"providerDescription\":null,\"passwordPolicy\":null,\"lockoutPolicy\":null,\"disableInternalUserManagement\":false}",
    "id": "c509d9be-68c1-49aa-8855-0f3064049ce3",
    "originKey": "uaa",
    "name": "uaa",
    "active": true,
    "identityZoneId": "638c87a2-e620-4d02-ac8d-9d18cef7ab00"
  }'