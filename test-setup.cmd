@ECHO OFF

SET BASEDIR=%~dp0

PUSHD %BASEDIR%

IF NOT EXIST pyenv\NUL GOTO NOENVDIR
ECHO removing existing environment
RMDIR /S /Q pyenv
:NOENVDIR

virtualenv pyenv
CALL pyenv\Scripts\activate
pip install mechanicalsoup
pip install -r pyenv.pkgs
CALL pyenv\Scripts\deactivate

POPD
