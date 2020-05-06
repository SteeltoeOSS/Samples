#!/usr/bin/env bash

set -e

basedir=$(dirname $0)
framework_init_flag=$basedir/.framework-initialized

command_available() {
  local cmd=$1
  command -v $cmd >/dev/null
}

# ensure pipenv available
if ! command_available pipenv >/dev/null; then
  echo "installing 'pipenv'"
  pip=pip
  command_available pip3 >/dev/null && pip=pip3
  $pip install pipenv --user
fi

# set working dir
cd $basedir

# initialize framework if needed
if [ ! -f $framework_init_flag ]; then
  pipenv install --ignore-pipfile
  touch $framework_init_flag
fi

# run samples
exec pipenv run behave $*
