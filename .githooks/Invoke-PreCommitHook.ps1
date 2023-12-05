$modelsOutOfDate = Test-Path (Join-Path $PSScriptRoot -ChildPath "..\GovUk.Frontend.Umbraco.ExampleApp\Models\ModelsBuilder\ood.flag")
If ($modelsOutOfDate) { 
    Write-Warning "Umbraco Models Builder models are out-of-date. Go to Settings > Models Builder > Generate models in the Umbraco backoffice to update them."; Exit 1 
}
else { Exit 0 }