#!/usr/bin/env bash

set -e

basedir=$(dirname $0)
reinit_flag=$basedir/reinit
PATH+=:~/.local/bin
PYTHON=${PYTHON:-python3.7}

command_available() {
  local cmd=$1
  command -v $cmd >/dev/null
}

env_exists() {
  pipenv --venv >/dev/null 2>&1
}

# ensure pipenv available
if ! command_available pipenv >/dev/null; then
  echo "installing 'pipenv'"
  $PYTHON -m pip install --user pipenv
fi

# set working dir
cd $basedir


# initialize framework if requested/needed
if [ -f $reinit_flag ] ; then
  echo "reinitializing"
  pipenv --rm || true
  rm $reinit_flag
fi
if ! env_exists; then
  echo "installing env"
  pipenv --python $PYTHON sync
fi

# run samples
exec pipenv run behave $*
