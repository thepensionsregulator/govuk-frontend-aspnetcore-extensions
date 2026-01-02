# Run tests

Install [Node.js](https://nodejs.org/en) before running the following commands.

To run unit tests on the client-side validation JavaScript:

```cmd
npm install
npm test
```

To run unit tests on the .NET code:

```cmd
npm run govuk
dotnet test
```

> Visual Studio may not run `ThePensionsRegulator.GovUk.Frontend.ConformanceTests` correctly the first time. Right-click the project and select 'Rebuild' to fix this.

Install [Pester](https://pester.dev/docs/quick-start) before running the following command.

To run unit tests on the PowerShell scripts:

```pwsh
Invoke-Pester
```

## Troubleshooting

### UNABLE_TO_GET_ISSUER_CERT_LOCALLY

If you get an error `UNABLE_TO_GET_ISSUER_CERT_LOCALLY` when running npm commands you need to click the padlock next to any site in the address bar of your browser, and download the CA certificate for your network in .PEM format. Then set the environment variable NODE_EXTRA_CA_CERTS to the path to that certificate, and restart Visual Studio.
