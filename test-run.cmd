@ECHO OFF

SET BASEDIR=%~dp0

PUSHD %BASEDIR%

CALL pyenv\Scripts\activate
CALL behave %*
CALL pyenv\Scripts\deactivate

POPD
