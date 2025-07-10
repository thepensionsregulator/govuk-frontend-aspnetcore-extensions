# Run tests

Install [Node.js](https://nodejs.org/en) before running the following commands.

To run unit tests on the client-side validation JavaScript:

```cmd
npm install
npm test
```

To run unit tests on the .NET code:

```cmd
dotnet test
```

> Visual Studio may not run `GovUk.Frontend.AspNetCore.Extensions.ConformanceTests` correctly the first time. Right-click the project and select 'Rebuild' to fix this.

Install [Pester](https://pester.dev/docs/quick-start) before running the following command.

To run unit tests on the PowerShell scripts:

```pwsh
Invoke-Pester
```
