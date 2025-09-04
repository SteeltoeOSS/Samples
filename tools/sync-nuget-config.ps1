# This script copies the nuget.config file from the repository root into the sample directories.
# While IDEs typically ignore the copies, they are needed for cf push.

Set-StrictMode -version 2.0
$ErrorActionPreference = 'Stop'

$repoRoot = "$PSScriptRoot/.."
$sourceFilePath = Resolve-Path -Path "$repoRoot/nuget.config"

foreach ($targetFilePath in Get-ChildItem -Path $repoRoot -Recurse -Filter 'nuget.config') {
    if ([string]$targetFilePath -ne $sourceFilePath) {
        $targetDir = Split-Path -Parent $targetFilePath
        Copy-Item $sourceFilePath $targetDir -Force
    }
}
