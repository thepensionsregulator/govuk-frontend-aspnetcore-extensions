param(
    # Path, relative to the root of the repo, to the file containing the commit message
    [string]$CommitMessageFile
)

# Add custom commands for this repo here
Invoke-Expression -Command "./.githooks/Confirm-AzureDevOpsWorkItem.ps1 $CommitMessageFile"
If ($LASTEXITCODE -gt 0) { Exit $LASTEXITCODE }