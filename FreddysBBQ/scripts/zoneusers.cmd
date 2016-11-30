:: vars to define
:: ZONE_ENDPOINT eg freddys-bbq.login.systemdomain.com
:: ZONEADMIN_CLIENT_ID 
:: ZONEADMIN_CLIENT_SECRET
SET ZONE_ENDPOINT=https://auth.login.system.testcloud.com
SET ZONEADMIN_CLIENT_ID=adminclient
SET ZONEADMIN_CLIENT_SECRET=adminsecret
cmd /c uaac target %ZONE_ENDPOINT% --skip-ssl-validation
cmd /c uaac token client get %ZONEADMIN_CLIENT_ID% -s %ZONEADMIN_CLIENT_SECRET%
cmd /c uaac user add frank --email frank@whitehouse.gov --given_name Frank --family_name Underwood -p password
cmd /c uaac user add freddy --email freddy@freddysbbq.com --given_name Freddy --family_name Hayes -p password
cmd /c uaac group add menu.read
cmd /c uaac group add menu.write
cmd /c uaac group add order.admin
cmd /c uaac group add order.me
cmd /c uaac member add menu.read frank
cmd /c uaac member add menu.read freddy
cmd /c uaac member add menu.write freddy
cmd /c uaac member add order.admin freddy
cmd /c uaac member add order.me frank
