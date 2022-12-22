# govuk-frontend-aspnetcore-extensions

This builds on [ASP.NET Core MVC tag helpers for GOV.UK Design System](https://github.com/gunndabad/govuk-frontend-aspnetcore) by James Gunn, adding support for:

- ASP.NET client-side validation using [jQuery Unobtrusive Validation](https://github.com/aspnet/jquery-validation-unobtrusive)
- Additional components:
  - [Task list summary](/docs/components/task-list-summary.md)
  - [Task list](/docs/components/task-list.md)
  - Back to top
  - Back to menu
  - TPR Header
  - TPR Context Bar
  - TPR Footer
- Adding non-interactive components (for example [details](https://design-system.service.gov.uk/components/details/) and [inset text](https://design-system.service.gov.uk/components/inset-text/)) entirely in Umbraco
- Configuring the text for interactive components (for example [text input](https://design-system.service.gov.uk/components/text-input/)) in Umbraco

## ASP.NET projects without Umbraco

- [Configure a new ASP.NET project](docs/aspnet/new-aspnet-project.md)
- [Localisation and validation in ASP.NET projects](docs/aspnet/localisation-and-validation.md)

## Umbraco

- [Run the Umbraco example application](docs/umbraco/run-example-application.md)
- [Configure a new Umbraco project](docs/umbraco/new-umbraco-project.md)
- [Populate initial values](docs/umbraco/populate-initial-values.md)
- [Validation](docs/umbraco/validation.md)
- [Filter the block list](docs/umbraco/filter-blocks.md)
- [Override property values](docs/umbraco/override-property-values.md)

## Tests

To run unit tests on the client-side validation JavaScript:

```cmd
yarn install
yarn jest
```

To run unit tests on the .NET code:

```cmd
dotnet test
```
