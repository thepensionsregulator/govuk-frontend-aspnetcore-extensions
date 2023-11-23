# GOV.UK Design System for ASP.NET MVC and Umbraco

This builds on [ASP.NET Core MVC tag helpers for GOV.UK Design System](https://github.com/gunndabad/govuk-frontend-aspnetcore) by James Gunn.

We add support for:

- ASP.NET client-side validation using [jQuery Unobtrusive Validation](https://github.com/aspnet/jquery-validation-unobtrusive)

- Adding the following non-interactive components entirely in Umbraco:

  - [Details](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/details.md)
  - [Error summary](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/error-summary.md)
  - [Fieldset](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/fieldset.md)
  - [Inset text](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/inset-text.md)
  - [Notification banner](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/notification-banner.md)
  - [Panel](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/panel.md)
  - [Summary list](/docs/components/summary-list.md)
  - [Warning text](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/warning-text.md)

- Configuring the text for the following components in Umbraco:

  - [Button](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/button.md)
  - [Checkboxes](/docs/components/checkboxes.md)
  - [Character count](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/character-count.md)
  - [Date input](/docs/components/date-input.md)
  - [Error message](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/error-message.md)
  - [File upload](/docs/components/file-upload.md)
  - [Pagination](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/pagination.md)
  - [Phase banner](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/phase-banner.md)
  - [Radios](/docs/components/radios.md)
  - [Select](/docs/components/select.md)
  - [Skip link](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/skip-link.md)
  - [Textarea](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/textarea.md)
  - [Text input](/docs/components/text-input.md)

- Additional components:

  - [Summary card](/docs/components/summary-card.md)
  - [Task list summary](/docs/components/task-list-summary.md)
  - [Task list](/docs/components/task-list.md)
  - [TPR back to top](/docs/components/tpr-back-to-top.md)
  - [TPR back to menu](/docs/components/tpr-back-to-menu.md)
  - [TPR header bar](/docs/components/tpr-header-bar.md)
  - [TPR context bar](/docs/components/tpr-context-bar.md)
  - [TPR footer bar](/docs/components/tpr-footer-bar.md)

- The Pensions Regulator (TPR) styling for all of the above components, and:
  - [Back link](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/back-link.md)

We target [GDS Frontend v4.4.1](https://github.com/alphagov/govuk-frontend/releases/tag/v4.4.1) in line with James Gunn's base project. We also include 'Summary card' from [GDS Frontend v4.5.0](https://github.com/alphagov/govuk-frontend/releases/tag/v4.5.0) and 'Task list' based on [govuk-frontend pre-release code](https://github.com/alphagov/govuk-design-system/pull/1994).

## ASP.NET projects without Umbraco

- [Configure a new ASP.NET project](docs/aspnet/new-aspnet-project.md)
- [Localisation and validation in ASP.NET projects](docs/aspnet/localisation-and-validation.md)
- [Use SASS for CSS](docs/aspnet/sass.md)

ASP.NET support for GOV.UK Design System components is published on NuGet as [ThePensionsRegulator.GovUk.Frontend](https://www.nuget.org/packages/ThePensionsRegulator.GovUk.Frontend)

![ThePensionsRegulator.GovUk.Frontend on nuget.org](https://img.shields.io/nuget/vpre/ThePensionsRegulator.GovUk.Frontend)

ASP.NET support for The Pensions Regulator components, and The Pensions Regulator styling of GOV.UK components, is published on NuGet as [ThePensionsRegulator.Frontend](https://www.nuget.org/packages/ThePensionsRegulator.Frontend)

![ThePensionsRegulator.Frontend on nuget.org](https://img.shields.io/nuget/vpre/ThePensionsRegulator.Frontend)

## Building applications with Umbraco

We provide features for building applications with Umbraco, particularly for working with block lists. These are not dependent on the GOV.UK Design System (but some examples do refer to GOV.UK Design System components).

- [Run the Umbraco example application](docs/umbraco/run-example-application.md)
- [Filter the block list](docs/umbraco/filter-blocks.md)
- [Format property values](docs/umbraco/format-property-values.md)
- [Override property values](docs/umbraco/override-property-values.md)
- [Umbraco unit testing](docs/umbraco/unit-testing.md)

Umbraco features not dependent upon the GOV.UK Design System are published on NuGet as [ThePensionsRegulator.Umbraco](https://www.nuget.org/packages/ThePensionsRegulator.Umbraco)

![ThePensionsRegulator.Umbraco on nuget.org](https://img.shields.io/nuget/vpre/ThePensionsRegulator.Umbraco)

Umbraco unit-testing support is published on NuGet as [ThePensionsRegulator.Umbraco.Testing](https://www.nuget.org/packages/ThePensionsRegulator.Umbraco.Testing)

![ThePensionsRegulator.Umbraco.Testing on nuget.org](https://img.shields.io/nuget/vpre/ThePensionsRegulator.Umbraco.Testing)

## Using the GOV.UK Design System in Umbraco

- [Configure a new Umbraco project](docs/umbraco/new-umbraco-project.md)
- [Populate initial values](docs/umbraco/populate-initial-values.md)
- [Validation](docs/umbraco/validation.md)
- [Use SASS for CSS](docs/aspnet/sass.md)
- [Configure the rich text editor](/docs/umbraco/rich-text-editor.md)

Umbraco GOV.UK Design System support is published on NuGet as [ThePensionsRegulator.GovUk.Frontend.Umbraco](https://www.nuget.org/packages/ThePensionsRegulator.GovUk.Frontend.Umbraco)

![ThePensionsRegulator.GovUk.Frontend.Umbraco on nuget.org](https://img.shields.io/nuget/vpre/ThePensionsRegulator.GovUk.Frontend.Umbraco)

## Disclaimer

This is a community implementation of the GOV.UK Design System. The Design System team is not responsible for it and cannot support you with using it. [Open an issue](https://github.com/thepensionsregulator/govuk-frontend-aspnetcore-extensions/issues) if you need help or you want to request a feature.
