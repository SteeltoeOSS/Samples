#!/usr/bin/env bash

set -x

cat <<EOF > Samples/user.ini
[behave.userdata]
max_attempts=$MAX_ATTEMPTS
cf_apiurl=$CF_APIURL
cf_username=$CF_USERNAME
cf_password=$CF_PASSWORD
cf_org=$CF_ORG
cf_max_attempts=$CF_MAX_ATTEMPTS
EOF
cat Samples/user.ini | grep -v 'password='

pushd Samples
  cp logging.ini logging.ini.orig
  sed -i 's/INFO/DEBUG/' logging.ini
  behave --format plain $FEATURES
  RC=$?
popd

cp Samples/test.log logs/$LOGFILE

exit $RC

# vim: ft=sh et
