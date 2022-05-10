# Copy this file to the root of your consuming repository. 
# It calls a script within the Tools-TPRGitHooks repository to set up hooks in your consuming repository.
Push-Location $PSScriptRoot

$expectedPath = (Resolve-Path "..") 
$expectedPath = "$expectedPath\Tools-TPRGitHooks"
Write-Output $expectedPath
If (Test-Path $expectedPath -PathType Container) {
    Push-Location $expectedPath
    & git pull
    .\install\Initialize-TprGitHooks.ps1 $PSScriptRoot
    Pop-Location
}
else {
    Write-Error "Tools-TPRGitHooks repo not found. Please clone it from Azure DevOps into $expectedPath and try again."
}

Pop-Location