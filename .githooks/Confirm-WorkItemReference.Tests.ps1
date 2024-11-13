Describe 'Confirm-WorkItemReference' {
    BeforeAll {
        Push-Location $PSScriptRoot
        $file = "./test-commit.txt"
        Mock Write-Warning {}
    }

    It 'Should allow a commit with an Azure DevOps work item reference' {
        Set-Content -Path $file -Value "Test commit AB#123456"
        ./Confirm-WorkItemReference.ps1 $file
        $LASTEXITCODE | Should -Be 0
    }
    
    It 'Should allow a commit with a Github Issues work item reference' {
        Set-Content -Path $file -Value "Test commit #123"
        ./Confirm-WorkItemReference $file 
        $LASTEXITCODE | Should -Be 0
    }

    It 'Should block a commit with a Azure DevOps work item reference without AB' {
        Set-Content -Path $file -Value "Test commit #123456"
        ./Confirm-WorkItemReference $file 
        $LASTEXITCODE | Should -Be 1
    }

    It 'Should allow a merge commit without a work item reference' {
        Set-Content -Path $file -Value "Merge branch 'example' into 'develop'"
        ./Confirm-WorkItemReference $file 
        $LASTEXITCODE | Should -Be 0
    }

    It 'Should block a non-merge commit without a work item reference' {
        Set-Content -Path $file -Value "Forgotten work item"
        ./Confirm-WorkItemReference $file 
        $LASTEXITCODE | Should -Be 1
    }

    AfterAll {
        Remove-Item $file
        Pop-Location
    }
}