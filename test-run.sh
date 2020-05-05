#!/usr/bin/env bash

set -e

basedir=$(dirname $0)
export PYTHONPATH="$basedir"/pylib

cd $basedir

pipenv run behave $*
