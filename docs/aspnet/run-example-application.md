# Run the ASP.NET example application

This repository includes an example application which demonstrates the validation working both client-side and server-side, and localisation using resource files.

1. Ensure you have .NET 10 installed.
2. Clone this repo
3. Run `npm run govuk` to install the `govuk-frontend` npm package.
4. Open `GovUk.Frontend.slnx` in Visual Studio 2026 or better, click on the `GovUk.Frontend.ExampleApp` project, and run it.

By default the example application uses The Pensions Regulator (TPR) branding. To see the GOV.UK branded version set `TPRStyles: false` in `appsettings.json` and re-run the application.

## Troubleshooting

### UNABLE_TO_GET_ISSUER_CERT_LOCALLY

If you get an error `UNABLE_TO_GET_ISSUER_CERT_LOCALLY` when running npm commands you need to click the padlock next to any site in the address bar of your browser, and download the CA certificate for your network in .PEM format. Then set the environment variable NODE_EXTRA_CA_CERTS to the path to that certificate, and restart Visual Studio.
