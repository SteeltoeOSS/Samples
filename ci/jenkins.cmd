@ECHO OFF

SET BASEDIR=%~dp0..
SET USERINI=%BASEDIR%\user.ini
FOR /F "tokens=1,2 delims=:" %%a in ("%STEELTOE_PCF_CREDENTIALS%") do (
   set CF_USER=%%a
   set CF_PASS=%%b
)

SET PYTHON_HOME=C:\Python36
SET MAVEN_HOME=C:\opt\apache\apache-maven-3.3.9

ECHO [behave.userdata] > %USERINI%
ECHO cf_apiurl = api.run.pcfbeta.io >> %USERINI%
ECHO cf_org = STEELTOE >> %USERINI%
ECHO cf_username = %CF_USER% >> %USERINI%
ECHO cf_password = %CF_PASS% >> %USERINI%

SET PATH=%PYTHON_HOME%\Scripts;%MAVEN_HOME%\bin;%PATH%

CALL %BASEDIR%\test-setup
CALL %BASEDIR%\test-run %*
