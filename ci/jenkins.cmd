@ECHO OFF

SET BASEDIR=%~dp0..
SET USERINI=%BASEDIR%\user.ini
FOR /F "tokens=1,2 delims=:" %%a in ("%STEELTOE_PCF_CREDENTIALS%") do (
   set CF_USER=%%a
   set CF_PASS=%%b
)

SET PYTHON_HOME=C:\Python36
SET DOTNET_HOME=C:\Program Files\dotnet
SET JAVA_HOME=C:\opt\oracle-jdk-8
SET RUBY_HOME=C:\tools\ruby25
SET GIT_HOME=C:\Program Files\Git
SET MAVEN_HOME=C:\opt\apache-maven-3.3.9
SET CF_HOME=C:\ProgramData\chocolatey\bin
SET TEST_OUT=C:\st\%~nx1

ECHO [behave.userdata] > %USERINI%
ECHO cf_apiurl = api.run.pcfone.io >> %USERINI%
ECHO cf_org = group-steeltoe >> %USERINI%
ECHO cf_username = %CF_USER% >> %USERINI%
ECHO cf_password = %CF_PASS% >> %USERINI%
ECHO cf_max_attempts = 250 >> %USERINI%
REM ECHO output = %TEST_OUT% >> %USERINI%

SET PATH=%PYTHON_HOME%;%PYTHON_HOME%\Scripts;%DOTNET_HOME%;%CF_HOME%;%JAVA_HOME%\bin;%RUBY_HOME%\bin;%GIT_HOME%\bin;%MAVEN_HOME%\bin;%PATH%
PATH

RMDIR /S /Q %TEST_OUT%
REM CALL %BASEDIR%\test-setup
REM CALL %BASEDIR%\test-run %*
behave %*
