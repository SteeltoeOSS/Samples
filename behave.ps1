$BaseDir = $PSScriptRoot
$ReInitFlag = "reinit"
$OldPath = $Env:Path

function Find-Python {
    $candidates = @("python3", "python", "py")
    foreach ($cmd in $candidates) {
        try {
            $python = & $cmd -c "import sys; print(sys.executable)" 2>$null
            if ($python) { return $cmd }
        }
        catch {}
    }
    throw "Python 3 not found on system."
}

function Command-Available {
    param($Command)
    try {
        Get-Command $Command -ErrorAction Stop | Out-Null
        return $true
    }
    catch {
        return $false
    }
}

function Env-Exists {
    param($PythonCmd)
    & $PythonCmd -m pipenv --venv 2>&1 | Out-Null
    return $?
}

$PythonCmd = Find-Python

# Ensure pipenv is installed
if (-not (Command-Available pipenv)) {
    Write-Host "Installing pipenv"
    & $PythonCmd -m pip install pipenv --user
}

try {
    Push-Location $BaseDir

    if (Test-Path $ReInitFlag) {
        Write-Host "Reinitializing"
        & $PythonCmd -m pipenv --rm
        Remove-Item $ReInitFlag
    }

    if (-not (Env-Exists $PythonCmd)) {
        Write-Host "Creating pipenv with $PythonCmd"
        & $PythonCmd -m pipenv --python $PythonCmd
        & $PythonCmd -m pipenv install
    }

    & $PythonCmd -m pipenv run behave @Args
}
finally {
    Pop-Location
    $Env:Path = $OldPath
}
