#!/bin/bash
set -ex
# vars to define
# UAA_ENDPOINT eg uaa.systemdomain.com
UAA_ENDPOINT=uaa.systemdomain.com
# from Ops Manager/ERT(renamed to PAS in PCF 2.0) Credentials, under UAA -> Admin Client
ADMIN_CLIENT_ID=admin
# ADMIN_CLIENT_SECRET get this from ops manager
ADMIN_CLIENT_SECRET=n3JwJQHCxFdb8wyOdNfvZK9L0zkUJ4-F
# set an id for the client we're going to use create to interact with credhub
UAA_CLIENT_ID=CredHubDemoId
# set a password for the client we're going to use create to interact with credhub
UAA_CLIENT_SECRET=CredHubDemoSecret

uaac target $UAA_ENDPOINT --skip-ssl-validation
uaac token client get $ADMIN_CLIENT_ID -s $ADMIN_CLIENT_SECRET
uaac client add $UAA_CLIENT_ID --authorities "credhub.read credhub.write" --authorized_grant_types client_credentials,password -s $UAA_CLIENT_SECRET