#!/bin/sh

maven_home=/opt/jenkins/data/tools/hudson.tasks.Maven_MavenInstallation/maven35/apache-maven-3.5.0

basedir=`dirname $0`/..
userini=$basedir/user.ini

cat > $userini <<EOF
[behave.userdata]
cf_apiurl = api.run.pcfone.io
cf_org = group-steeltoe
cf_username = `echo $STEELTOE_PCF_CREDENTIALS | cut -d: -f1`
cf_password = `echo $STEELTOE_PCF_CREDENTIALS | cut -d: -f2`
cf_max_attempts = 250
EOF


PATH=$maven_home/bin:$PATH

$basedir/test-setup
$basedir/test-run $*
