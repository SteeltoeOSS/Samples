#!/usr/bin/env bash

set -e
prog=$(basename $0)
base_dir="$(dirname $0)"/..
source "$base_dir"/.libexec/functions.sh
pipeline=azure-pipeline.yaml

# ----------------------------------------------------------------------------
# help
# ----------------------------------------------------------------------------

usage() {
  cat<<EOF
USAGE
      $prog [OPTION] BRANCH [BRANCH...]

DESCRIPTION
     Update sample pipelines.
     For each sample that has a feature file, create a corresponding pipeline.

WHERE
     BRANCH  One or branch names

OPTIONS
     -h
     --help
             Print this message.

     -l
     --list
             List samples that would be updated, but do not update.
EOF
}

# ----------------------------------------------------------------------------
# args
# ----------------------------------------------------------------------------

do_list=false
branches=()
while [ $# -gt 0 ]; do
  case $1 in
    -h|--help)
      usage
      exit
      ;;
    -l|--list)
      do_list=true
      shift
      ;;
    -*)
      die "$1 is not a valid option; run with -h for help"
      ;;
    *)
      branches+=($1)
      shift
      ;;
  esac
done

# ----------------------------------------------------------------------------
# funcs
# ----------------------------------------------------------------------------

get_sample_paths() {
  find "$base_dir" -name '*.feature' -exec dirname {} \;| sed 's|^'"$base_dir"'/||'
}

# ----------------------------------------------------------------------------
# main
# ----------------------------------------------------------------------------

if $do_list; then
  for sample_path in $(get_sample_paths); do
    echo $sample_path
  done
  exit
fi

[ ${#branches[@]} -gt 0 ] || die "no branches specified; run with -h for help"

for sample_path in $(get_sample_paths); do
  msg "updating $sample_path"
  sample_pipeline=$sample_path/$pipeline
  feature=$(expr "$sample_path" : '\([^/]*\)')
  sample=$(basename $sample_path)
  rm -f $sample_pipeline
  cat > $sample_pipeline << EOF
trigger:
  branches:
    include:
EOF
  for branch in ${branches[@]}; do
    cat >> $sample_pipeline << EOF
      - $branch
EOF
  done
  cat >> $sample_pipeline << EOF
  paths:
    include:
      - ci/*
      - environment.py
      - pylib/*
      - $sample_path/*

variables:
  - group: 'Samples Configuration and Credentials'

jobs:
  -
    template: ../../../../ci/templates/cloud-foundry-job.yml
    parameters:
      feature: $feature
      sample: $sample
EOF
done
