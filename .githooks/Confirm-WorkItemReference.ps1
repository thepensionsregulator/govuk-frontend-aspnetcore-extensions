param(
    # Path, relative to the root of the repo, to the file containing the commit message
    [string]$CommitMessageFile
)

# Check for a work item reference, either Azure DevOps or Github
$commitMessage = (Get-Content $CommitMessageFile);
If (!($commitMessage -Match "AB#\d{6}" -Or $commitMessage -Match "#\d{3,4}") -And !($commitMessage -Match "^Merge branch '")) { 
    Write-Warning "Commit aborted. Your commit message must include a Azure DevOps work item reference, eg AB#12345"; Exit 1 
}
else { Exit 0 }