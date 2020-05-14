#!/usr/bin/env bash

set -e

prog=$(basename $0)
base_dir=$(cd "$(dirname $0)"/.. && pwd)
ci_dir="$base_dir"/ci

source "$base_dir"/.libexec/functions.sh

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

get_samples() {
  find "$base_dir" -name '*.feature' -exec dirname {} \; | sed 's|^'"$base_dir"'/||'
}

get_sample_name() {
  basename $1
}

get_sample_feature() {
  expr $1 : '\([^/]*\)'
}

tolower() {
  echo "$*" | tr [:upper:] [:lower:]
}

# ----------------------------------------------------------------------------
# main
# ----------------------------------------------------------------------------

if $do_list; then
  for sample in $(get_samples); do
    echo $sample
  done
  exit
fi

[ ${#branches[@]} -gt 0 ] || die "no branches specified; run with -h for help"

for sample in $(get_samples); do
  name=$(get_sample_name $sample)
  feature=$(get_sample_feature $sample)
  pipeline=azure-pipeline-$(tolower $feature)-$(tolower $name).yaml
  msg "updating $sample -> $pipeline"
  pipeline_path="$ci_dir"/$pipeline
  if [[ $feature:$name == Security:CloudFoundrySingleSignon ]]; then
    template=cloud-foundry-uaac-job.yaml
  else
    template=cloud-foundry-job.yaml
  fi
  cat > "$pipeline_path" << EOF
trigger:
  branches:
    include:
EOF
  for branch in ${branches[@]}; do
    cat >> "$pipeline_path" << EOF
      - $branch
EOF
  done
  cat >> "$pipeline_path" << EOF
  paths:
    include:
      - config/*
      - $sample/*

variables:
  - group: 'Samples Configuration and Credentials'

jobs:
  -
    template: templates/$template
    parameters:
      feature: $feature
      sample: $name
EOF
done
