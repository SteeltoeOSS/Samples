:: This script is not intended to run in powershell (change the outer double quotes to single quotes on the -d parameter in the uaac curl command if you want to run from powershell)
:: variables to define
:: eg uaa.systemdomain.com
SET UAA_ENDPOINT=uaa.systemdomain.com
:: from Ops Manager/ERT(renamed to PAS in PCF 2.0) Credentials, under UAA -> Admin Client
SET ADMIN_CLIENT_ID=admin
:: also from ops manager
SET ADMIN_CLIENT_SECRET=LhCN75-pYpm-7M0HFVdSwLKDGnLPxUk1
:: this is the guid of the identity zone, it's the first guid in the URI for any page in the Pivotal SSO UI 
SET IDENTITY_ZONE_ID=1d94b717-2f59-4e07-82aa-66e28dc649b1
:: set an id for the zone-specific admin client we're going to create to be able to create groups and users
SET ZONEADMIN_CLIENT_ID=adminclient
:: set your own password for the zone client
SET ZONEADMIN_CLIENT_SECRET=adminsecret
:: ZONE_ENDPOINT eg auth.login.systemdomain.com
SET ZONE_ENDPOINT=https://auth.login.systemdomain.com
SET SSO_USER=dave
SET SSO_USER_PASSWORD=Password1!
SET SSO_USER_EMAIL=dave@testcloud.com
SET SSO_USER_FN=Dave
SET SSO_USER_LN=Tillman

cmd /c uaac target %UAA_ENDPOINT% --skip-ssl-validation
cmd /c uaac token client get %ADMIN_CLIENT_ID% -s %ADMIN_CLIENT_SECRET%
cmd /c uaac client add temp --authorities zones.write,scim.zones --scope zones.%IDENTITY_ZONE_ID%.admin --authorized_grant_types client_credentials,password -s %ZONEADMIN_CLIENT_SECRET%
cmd /c uaac user add temp --email cf-spring-cloud-services@pivotal.io -p %ZONEADMIN_CLIENT_SECRET%
cmd /c uaac member add zones.%IDENTITY_ZONE_ID%.admin temp
cmd /c uaac token owner get temp temp -p %ZONEADMIN_CLIENT_SECRET% -s %ZONEADMIN_CLIENT_SECRET%
:: get a token for the zone
cmd /c uaac curl /oauth/clients -k -H "Content-type:application/json" -H "X-Identity-Zone-Id:%IDENTITY_ZONE_ID%" -X POST -d "{\"client_id\":\"%ZONEADMIN_CLIENT_ID%\",\"client_secret\":\"%ZONEADMIN_CLIENT_SECRET%\",\"scope\":[\"uaa.none\"],\"resource_ids\":[\"none\"],\"authorities\":[\"uaa.admin\",\"clients.read\",\"clients.write\",\"scim.read\",\"scim.write\",\"clients.secret\"],\"authorized_grant_types\":[\"client_credentials\"]}"
cmd /c uaac token client get %ADMIN_CLIENT_ID% -s %ADMIN_CLIENT_SECRET%
:: cleanup temp user/client
cmd /c uaac user delete temp
cmd /c uaac client delete temp

:: switch target from UAA server to the SSO zone
cmd /c uaac target %ZONE_ENDPOINT% --skip-ssl-validation
:: get a token for the zone
cmd /c uaac token client get %ZONEADMIN_CLIENT_ID% -s %ZONEADMIN_CLIENT_SECRET%
:: finally add our user
cmd /c uaac user add %SSO_USER% --email %SSO_USER_EMAIL% --given_name %SSO_USER_FN% --family_name %SSO_USER_LN% -p %SSO_USER_PASSWORD%
:: add the group
cmd /c uaac group add testgroup
:: add the user to the group
cmd /c uaac member add testgroup %SSO_USER%