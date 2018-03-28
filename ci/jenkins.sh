#!/bin/sh

basedir=`dirname $0`/..
userini=$basedir/user.ini

cat > $userini <<EOF
[behave.userdata]
cf_apiurl = api.run.pcfbeta.io
cf_org = STEELTOE
cf_username = `echo $STEELTOE_PCF_CREDENTIALS | cut -d: -f1`
cf_password = `echo $STEELTOE_PCF_CREDENTIALS | cut -d: -f2`
EOF
