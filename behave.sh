#!/usr/bin/env bash

set -euo pipefail

basedir=$(dirname "$0")
reinit_flag="$basedir/reinit"
PATH+=:~/.local/bin

# Try to find a valid python3 interpreter
if command -v python3 >/dev/null; then
  PYTHON=$(command -v python3)
elif command -v python >/dev/null; then
  PYTHON=$(command -v python)
else
  echo "Error: Python 3 not found"
  exit 1
fi

# Ensure pipenv is installed
if ! command -v pipenv >/dev/null; then
  echo "Installing pipenv"
  "$PYTHON" -m pip install --user pipenv
fi

cd "$basedir"

# Reinitialize environment if requested
if [ -f "$reinit_flag" ]; then
  echo "Reinitializing"
  pipenv --rm || true
  rm "$reinit_flag"
fi

# Create environment with the detected Python interpreter
if ! pipenv --venv >/dev/null 2>&1; then
  echo "Creating pipenv with $PYTHON"
  pipenv --python "$PYTHON"
  pipenv sync
fi

# Run behave tests
exec pipenv run behave "$@"
