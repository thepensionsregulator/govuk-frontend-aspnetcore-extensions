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

## Getting started

For .NET projects without Umbraco see [GovUk.Frontend.AspNetCore.Extensions](GovUk.Frontend.AspNetCore.Extensions/README.md).

For Umbraco projects see [GovUk.Frontend.Umbraco](GovUk.Frontend.Umbraco/README.md).

## Run the Umbraco example application

1. Ensure you have .NET 6.0.5 (SDK version 6.0.300) or better installed. Run `dotnet --list-sdks` to check.
2. Clone this repo
3. Clone the `govuk-frontend` submodule:

   ```pwsh
   git submodule update --init
   ```

4. Open `GovUk.Frontend.sln` in Visual Studio 2022 or better, click on the `GovUk.Frontend.Umbraco.ExampleApp` project, and run it.
5. When you see the Umbraco installer enter your name, email address and a new password and click 'Continue'.
6. When you are taken to the Umbraco back office go to Settings > uSync > Everything > Import all
7. View the example application at https://localhost:44350

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
