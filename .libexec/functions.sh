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
