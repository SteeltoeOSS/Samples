#!/usr/bin/env bash

set -e

prog=$(basename $0)

msg() {
  tput setaf 2
  echo "--- $*"
  tput sgr0
}

err() {
  tput setaf 1
  echo "!!! $*" >&2
  tput sgr0
}

die() {
  err $*
  exit 1
}

# ----------------------------------------------------------------------------
# help
# ----------------------------------------------------------------------------

usage() {
  cat<<EOF
USAGE
      $prog [OPTION]

DESCRIPTION
     tbd

WHERE
     tbd
             tbd

OPTIONS
     -d space
     --delete=space
             Delete named space.

     -D
     --delete-all-spaces
             Delete all spaces.

     -h
     --help
             Print this message.

     -i space
     --info=space
             Print info about named space.

     -l
     --list
             List spaces.
EOF
}

# ----------------------------------------------------------------------------
# args
# ----------------------------------------------------------------------------

do_delete=false
do_delete_all=false
do_info=false
do_list=false
space=
while [ $# -gt 0 ]; do
  case $1 in
    -d|--delete)
      shift
      [ $# -eq 0 ] && die "space not specified; run with -h for help"
      do_delete=true
      space=$1
      shift
      ;;
    --delete=*)
      do_delete=true
      space=$(echo $1 | cut -d= -f2)
      [ -z "$space" ] && die "space not specified; run with -h for help"
      shift
      ;;
    -D|--delete-all-spaces)
      shift
      do_delete_all=true
      ;;
    -h|--help)
      usage
      exit
      ;;
    -i|--info)
      shift
      [ $# -eq 0 ] && die "space not specified; run with -h for help"
      do_info=true
      space=$1
      shift
      ;;
    --info=*)
      do_info=true
      space=$(echo $1 | cut -d= -f2)
      [ -z "$space" ] && die "space not specified; run with -h for help"
      shift
      ;;
    -l|--list)
      do_list=true
      shift
      ;;
    -*)
      die "$1 is not a valid option; run with -h for help"
      break
      ;;
    *)
      die "$1 is not a valid arg; run with -h for help"
      break
  esac
done

# ----------------------------------------------------------------------------
# funcs
# ----------------------------------------------------------------------------

get_spaces() {
  cf spaces | sort | awk '/^[[:alnum:]]{8}-[[:alnum:]]{4}-[[:alnum:]]{4}-[[:alnum:]]{4}-[[:alnum:]]{12}$/'
}

get_space() {
  cf target | grep '^space:' | awk '{print $2}'
}

set_space() {
  local space=$1
  cf target -s $space >/dev/null
}

delete_space() {
  local space=$1
  msg "deleting space $space"
  set_space $space
  for app in $(get_apps); do
    delete_app $app
  done
  for service in $(get_services); do
    delete_service $service
  done
  cf delete-space $space -f >/dev/null
}

get_services() {
  local space=$1
  cf services | tail +4 | awk '{print $1}'
}

delete_service() {
  local service=$1
  msg "  deleting service $service"
  while true; do
    status=$(cf service $service 2>/dev/null | grep '^status:' | sed 's/.*:[[:space:]]*\(.*\)/\1/')
    case $status in
      "")
        break
        ;;
      "create succeeded")
        cf delete-service $service -f >/dev/null
        ;;
      "delete in progress")
        ;;
      "delete failed")
        err "failed to delete service"
        break
        ;;
      *)
        err "unknown service status: $status"
        break
        ;;
    esac
  done
}

get_apps() {
  cf apps | tail +5 | awk '{print $1}'
}

delete_app() {
  local app=$1
  msg "  deleting app $app"
  cf delete $app -f -r >/dev/null 2>&1
}

# ----------------------------------------------------------------------------
# main
# ----------------------------------------------------------------------------

src_config=${CF_HOME:-$HOME}/.cf
[ -d $src_config ] || die "cf config does not exist: $src_config"
export CF_HOME=$TMPDIR/$prog.$$
mkdir $CF_HOME
cp -r $src_config $CF_HOME

trap "rm -rf $CF_HOME"  EXIT

if $do_list; then
  for space in $(get_spaces); do
    echo $space
  done
  exit
fi

if $do_info; then
  set_space $space
  msg "services"
  services=$(get_services)
  if [ -z "$services" ]; then
    echo "(none)"
  else
    for service in $services; do
      echo $service
    done
  fi
  msg "apps"
  apps=$(get_apps)
  if [ -z "$apps" ]; then
    echo "(none)"
  else
    for app in $apps; do
      echo $app
    done
  fi
  exit
fi

if $do_delete; then
  delete_space $space
  exit
fi

if $do_delete_all; then
  for space in $(get_spaces); do
    delete_space $space
  done
  exit
fi

die "no args specified; run with -h for help"
exit

for space in $spaces; do
  echo $space
done
echo
echo -n "this will detroy the above spaces; continue? [y/N] : "
read reply

[ "$reply" == "y" ] || [ "$reply" == "Y" ] || exit

for space in $spaces; do
  msg "targeting space $space"
  cf target -s $space >/dev/null
  # apps
  apps=$(cf apps | tail +5 | awk '{print $1}')
  if [[ -z "$apps" ]]; then
    msg "no apps found"
  else
    for app in $apps; do
      msg "deleting app $app"
      cf delete -f "$app"
      if [ $? -ne 0 ]; then
        err "failed to delete app $app"
      fi
    done
  fi
  # services
  services=$(cf services | tail +4 | awk '{print $1}')
  if [[ -z "$services" ]]; then
    msg "no services found"
  else
    for service in $services; do
      msg "deleting service $service"
      while true; do
        status=$(cf service $service | grep '^status:')
        echo "status -> $status"
        if [[ "$status" == *"delete in progress"* ]]; then
          sleep 1
          conintue
        fi
        if ! cf delete-service -f "$service"; then
          err "failed to delete service $app"
          continue
        fi
      done
    done
  fi
  msg "deleting space $space"
  cf delete-space -f $space
done
