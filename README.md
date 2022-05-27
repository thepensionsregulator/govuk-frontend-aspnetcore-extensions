# govuk-frontend-aspnetcore-extensions

This builds on [ASP.NET Core MVC tag helpers for GOV.UK Design System](https://github.com/gunndabad/govuk-frontend-aspnetcore) by James Gunn, adding support for:

- ASP.NET client-side validation using [jQuery Unobtrusive Validation](https://github.com/aspnet/jquery-validation-unobtrusive)
- Adding non-interactive components (for example [details](https://design-system.service.gov.uk/components/details/) and [inset text](https://design-system.service.gov.uk/components/inset-text/)) entirely in Umbraco
- Configuring the text for interactive components (for example [text input](https://design-system.service.gov.uk/components/text-input/)) in Umbraco

## Getting started

For .NET projects without Umbraco see [GovUk.Frontend.AspNetCore.Extensions](GovUk.Frontend.AspNetCore.Extensions/README.md).

For Umbraco projects see [GovUk.Frontend.Umbraco](GovUk.Frontend.Umbraco/README.md).

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
