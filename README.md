# govuk-frontend-aspnetcore-extensions

This builds on [ASP.NET Core MVC tag helpers for GOV.UK Design System](https://github.com/gunndabad/govuk-frontend-aspnetcore) by James Gunn, adding support for:

- ASP.NET client-side validation using [jQuery Unobtrusive Validation](https://github.com/aspnet/jquery-validation-unobtrusive)
- Additional components:
  - [Summary card](/docs/components/summary-card.md)
  - [Task list summary](/docs/components/task-list-summary.md)
  - [Task list](/docs/components/task-list.md)
  - [TPR back to top](/docs/components/tpr-back-to-top.md)
  - [TPR back to menu](/docs/components/tpr-back-to-menu.md)
  - [TPR header bar](/docs/components/tpr-header-bar.md)
  - [TPR context bar](/docs/components/tpr-context-bar.md)
  - [TPR footer bar](/docs/components/tpr-footer-bar.md)
- Adding non-interactive components (for example [details](https://design-system.service.gov.uk/components/details/) and [inset text](https://design-system.service.gov.uk/components/inset-text/)) entirely in Umbraco
- Configuring the text for interactive components (for example [text input](https://design-system.service.gov.uk/components/text-input/)) in Umbraco

We target [GDS Frontend v4.3.0](https://github.com/alphagov/govuk-frontend/releases/tag/v4.3.0) in line with James Gunn's base project. We also include 'Summary card' from [GDS Frontend v4.5.0](https://github.com/alphagov/govuk-frontend/releases/tag/v4.5.0) and 'Task list' based on [govuk-frontend pre-release code](https://github.com/alphagov/govuk-design-system/pull/1994).

## ASP.NET projects without Umbraco

- [Configure a new ASP.NET project](docs/aspnet/new-aspnet-project.md)
- [Localisation and validation in ASP.NET projects](docs/aspnet/localisation-and-validation.md)
- [Use SASS for CSS](docs/aspnet/sass.md)

ASP.NET support is published on NuGet as [ThePensionsRegulator.GovUk.Frontend](https://www.nuget.org/packages/ThePensionsRegulator.GovUk.Frontend)

![ThePensionsRegulator.GovUk.Frontend on nuget.org](https://img.shields.io/nuget/vpre/ThePensionsRegulator.GovUk.Frontend)

## Umbraco

- [Run the Umbraco example application](docs/umbraco/run-example-application.md)
- [Configure a new Umbraco project](docs/umbraco/new-umbraco-project.md)
- [Populate initial values](docs/umbraco/populate-initial-values.md)
- [Validation](docs/umbraco/validation.md)
- [Filter the block list](docs/umbraco/filter-blocks.md)
- [Override property values](docs/umbraco/override-property-values.md)
- [Umbraco unit testing](docs/umbraco/unit-testing.md)
- [Use SASS for CSS](docs/aspnet/sass.md)

Umbraco support is published on NuGet as [ThePensionsRegulator.GovUk.Frontend.Umbraco](https://www.nuget.org/packages/ThePensionsRegulator.GovUk.Frontend.Umbraco)

![ThePensionsRegulator.GovUk.Frontend.Umbraco on nuget.org](https://img.shields.io/nuget/vpre/ThePensionsRegulator.GovUk.Frontend.Umbraco)

Umbraco unit-testing support is published on NuGet as [ThePensionsRegulator.GovUk.Frontend.Umbraco.Testing](https://www.nuget.org/packages/ThePensionsRegulator.GovUk.Frontend.Umbraco.Testing)

![ThePensionsRegulator.GovUk.Frontend.Umbraco.Testing on nuget.org](https://img.shields.io/nuget/vpre/ThePensionsRegulator.GovUk.Frontend.Umbraco.Testing)

## Disclaimer

This is a community implementation of the GOV.UK Design System. The Design System team is not responsible for it and cannot support you with using it. [Open an issue](https://github.com/thepensionsregulator/govuk-frontend-aspnetcore-extensions/issues) if you need help or you want to request a feature.
