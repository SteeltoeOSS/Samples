#!/bin/bash
set -ex
# vars to define
# UAA_ENDPOINT eg uaa.systemdomain.com
UAA_ENDPOINT=uaa.systemdomain.com
# from Ops Manager/ERT(renamed to PAS in PCF 2.0) Credentials, under UAA -> Admin Client
ADMIN_CLIENT_ID=admin
# ADMIN_CLIENT_SECRET get this from ops manager
ADMIN_CLIENT_SECRET=LhCN75-pYpm-7M0HFVdSwLKDGnLPxUk1
# IDENTITY_ZONE_ID this is the guid of the identity zone, it’s the first guid in the URI for any page in the Pivotal SSO UI 
IDENTITY_ZONE_ID=1d94b717-2f59-4e07-82aa-66e28dc649b1
# ZONEADMIN_CLIENT_ID set an id for the zone-specific admin client we're going to create to be able to create groups and users
ZONEADMIN_CLIENT_ID=adminclient2
# ZONEADMIN_CLIENT_SECRET set your own password for the zone client
ZONEADMIN_CLIENT_SECRET=adminsecret2
# ZONE_ENDPOINT eg auth.login.systemdomain.com
ZONE_ENDPOINT=https://auth.login.systemdomain.com
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