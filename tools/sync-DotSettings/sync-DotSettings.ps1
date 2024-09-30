#Requires -Version 7.4

# This script copies the Resharper/Rider code style settings (.sln.DotSettings file) from Steeltoe, 
# patches it for use in samples, then distributes it across the samples. It enables IDE IntelliSense and code cleanup.

# https://stackoverflow.com/a/48877892
$ErrorActionPreference = "Stop"
$PSNativeCommandUseErrorActionPreference = $true

Copy-Item "../../../Steeltoe/src/Steeltoe.All.sln.DotSettings" -Destination "baseline.DotSettings"
git apply --unidiff-zero --recount DotSettings.patch

Get-ChildItem "../../" -Recurse -Filter "Steeltoe.Samples.*.sln" | ForEach-Object {
  $targetFileName = "$($_.Name).DotSettings"
  $targetPath = [System.IO.Path]::Combine([System.IO.Path]::GetDirectoryName($_.FullName), $targetFileName)
  Copy-Item "baseline.DotSettings" -Destination $targetPath
}
