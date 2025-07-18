
$ValidPath = Test-Path -Path .\tempnuget
if ($ValidPath) {
    rm -r -fo .\tempnuget
    New-Item .\tempnuget  -ItemType:Directory
}


$paths = @(
    ".\GovUk.Frontend.AspNetCore.Extensions\bin\Debug\*.*"
    ".\GovUk.Frontend.Umbraco\bin\Debug\*.*"
    ".\ThePensionsRegulator.Frontend\bin\Debug\*.*"
    ".\ThePensionsRegulator.Umbraco\bin\Debug\*.*"
    ".\ThePensionsRegulator.Frontend.Umbraco\bin\Debug\*.*"
    ".\ThePensionsRegulator.Umbraco\bin\Debug\*.*"
)

foreach ($path in $paths) {
    Copy-Item -Path:$path  -Destination .\tempnuget -Include "*.*u*" -Recurse:$false -Container:$true
}