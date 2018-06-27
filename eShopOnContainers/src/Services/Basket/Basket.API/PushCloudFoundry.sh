#! /bin/bash
rm -rf ./_publish
dotnet publish -f netcoreapp2.0 -r ubuntu.14.04-x64 -o ./_publish
cf push -f ./manifest.yml -p ./_publish