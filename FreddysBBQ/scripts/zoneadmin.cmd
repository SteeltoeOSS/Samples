:: vars to define
:: UAA_ENDPOINT eg uaa.systemdomain.com
:: ADMIN_CLIENT_ID eg admin
:: ADMIN_CLIENT_SECRET get this from ops manager
:: IDENTITY_ZONE_ID this is the guid of the identity zone, it’s the first guid in the URI for any page in the Pivotal SSO UI 
:: ZONEADMIN_CLIENT_ID pick a name for the admin client in the zone
:: ZONEADMIN_CLIENT_SECRET 
SET UAA_ENDPOINT=uaa.system.testcloud.com
SET ADMIN_CLIENT_ID=admin
SET ADMIN_CLIENT_SECRET=YhnYHCEzvIzvnW8A0U8fwV68-Mu026qA
SET IDENTITY_ZONE_ID=ecdbc700-50a9-44c3-8170-de4d5cc566c5
SET ZONEADMIN_CLIENT_ID=adminclient
SET ZONEADMIN_CLIENT_SECRET=adminsecret
cmd /c uaac target %UAA_ENDPOINT% --skip-ssl-validation
cmd /c uaac token client get %ADMIN_CLIENT_ID% -s %ADMIN_CLIENT_SECRET%
cmd /c uaac client add temp --authorities zones.write,scim.zones --scope zones.%IDENTITY_ZONE_ID%.admin --authorized_grant_types client_credentials,password -s %ZONEADMIN_CLIENT_SECRET%
cmd /c uaac user add temp --email cf-spring-cloud-services@pivotal.io -p %ZONEADMIN_CLIENT_SECRET%
cmd /c uaac member add zones.%IDENTITY_ZONE_ID%.admin temp
cmd /c uaac token owner get temp temp -p %ZONEADMIN_CLIENT_SECRET% -s %ZONEADMIN_CLIENT_SECRET%
cmd /c uaac curl /oauth/clients -k -H "Content-type:application/json" -H "X-Identity-Zone-Id:ecdbc700-50a9-44c3-8170-de4d5cc566c5" -X POST -d "{\"client_id\":\"adminclient\",\"client_secret\":\"adminsecret\",\"scope\":[\"uaa.none\"],\"resource_ids\":[\"none\"],\"authorities\":[\"uaa.admin\",\"clients.read\",\"clients.write\",\"scim.read\",\"scim.write\",\"clients.secret\"],\"authorized_grant_types\":[\"client_credentials\"]}"
cmd /c uaac token client get %ADMIN_CLIENT_ID% -s %ADMIN_CLIENT_SECRET%
cmd /c uaac user delete temp
cmd /c uaac client delete temp
