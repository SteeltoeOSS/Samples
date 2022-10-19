

$BaseDir = $PSScriptRoot
$ReInitFlag = "reinit"
$OldPath = $Env:Path
$Env:Path += ";$Env:AppData\Python\Python310\Scripts"

function Command-Available {
    param($Command)
    $OldPref = $ErrorActionPreference
    $ErrorActionPreference = 'stop'
    try {
        (Get-Command $Command)
        return $True
    }
    catch {
        return $False
    }
    finally {
        $ErrorActionPreference = $OldPref
    }
}

function Env-Exists {
    pipenv --venv 2>&1 | Out-Null
    $?
}

# ensure pipenv available
if (!(Command-Available pipenv)) {
    "installing 'pipenv'"
    pip3 install pipenv --user
}

try {
    # set working dir
    Push-Location $BaseDir

    # initialize framework if requested
    if (Test-Path $ReInitFlag) {
        "reinitializing"
        pipenv --rm
        Remove-Item $ReInitFlag
    }
    if (!(Env-Exists)) {
        "installing env"
         pipenv sync
    }

    # run samples
    pipenv run behave $Args
}
finally {
    Pop-Location
    $Env:Path = $OldPath
}

