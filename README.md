# GOV.UK Design System for ASP.NET MVC and Umbraco

This builds on [ASP.NET Core MVC tag helpers for GOV.UK Design System](https://github.com/gunndabad/govuk-frontend-aspnetcore) by James Gunn.

We add support for:

- ASP.NET client-side validation using [jQuery Unobtrusive Validation](https://github.com/aspnet/jquery-validation-unobtrusive)

- Adding the following non-interactive components entirely in Umbraco:

  - [Accordion](/docs/components/accordion.md)
  - [Details](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/details.md)
  - [Error summary](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/error-summary.md)
  - [Fieldset](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/fieldset.md)
  - [Inset text](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/inset-text.md)
  - [Notification banner](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/notification-banner.md)
  - [Panel](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/panel.md)
  - [Section break](https://design-system.service.gov.uk/styles/section-break/)
  - [Summary card](/docs/components/summary-card.md)
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

  - [Task list summary](/docs/components/task-list-summary.md)
  - [Task list](/docs/components/task-list.md)
  - [TPR back to top](/docs/components/tpr-back-to-top.md)
  - [TPR back to menu](/docs/components/tpr-back-to-menu.md)
  - [TPR box](/docs/components/tpr-box.md)
  - [TPR header bar](/docs/components/tpr-header-bar.md)
  - [TPR context bar](/docs/components/tpr-context-bar.md)
  - [TPR footer bar](/docs/components/tpr-footer-bar.md)
  - [TPR YouTube video](/docs/components/tpr-youtube-video.md)
  - [TPR related links](/docs/components/tpr-related-links.md)
  - [TPR section cards](/docs/components/tpr-section-cards.md)

- The Pensions Regulator (TPR) styling for all of the above components, and:
  - [Back link](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/back-link.md)
  - [Breadcrumbs](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/breadcrumbs.md)

We target [GOV.UK Frontend v5.2.0](https://github.com/alphagov/govuk-frontend/releases/tag/v5.2.0) in line with James Gunn's base project.

## ASP.NET projects without Umbraco

- [Run the ASP.NET example application](docs/aspnet/run-example-application.md)
- [Configure a new ASP.NET project (GOV.UK)](docs/aspnet/new-aspnet-project-govuk.md)
- [Configure a new ASP.NET project (TPR)](docs/aspnet/new-aspnet-project-tpr.md)
- [Localisation and validation in ASP.NET projects](docs/aspnet/localisation-and-validation.md)
- [Use SASS for CSS](docs/aspnet/sass.md)
- [Read the TPR consent cookie](docs/aspnet/consent-cookie.md)

ASP.NET support for GOV.UK Design System components is published on NuGet as [ThePensionsRegulator.GovUk.Frontend](https://www.nuget.org/packages/ThePensionsRegulator.GovUk.Frontend)

![ThePensionsRegulator.GovUk.Frontend on nuget.org](https://img.shields.io/nuget/vpre/ThePensionsRegulator.GovUk.Frontend)

ASP.NET support for The Pensions Regulator components, and The Pensions Regulator styling of GOV.UK components, is published on NuGet as [ThePensionsRegulator.Frontend](https://www.nuget.org/packages/ThePensionsRegulator.Frontend)

![ThePensionsRegulator.Frontend on nuget.org](https://img.shields.io/nuget/vpre/ThePensionsRegulator.Frontend)

## Building applications with Umbraco

We provide features for building applications with Umbraco, particularly for working with block grids and block lists. These are not dependent on the GOV.UK Design System (but some examples do refer to GOV.UK Design System components).

- [Run the Umbraco example application](docs/umbraco/run-example-application.md)
- [Filter the block list](docs/umbraco/filter-blocks.md)
- [Format property values](docs/umbraco/format-property-values.md)
- [Override property values](docs/umbraco/override-property-values.md)
- [Umbraco unit testing](docs/umbraco/unit-testing.md)

Umbraco features not dependent upon the GOV.UK Design System are published on NuGet as [ThePensionsRegulator.Umbraco](https://www.nuget.org/packages/ThePensionsRegulator.Umbraco)

![ThePensionsRegulator.Umbraco on nuget.org](https://img.shields.io/nuget/vpre/ThePensionsRegulator.Umbraco)

Umbraco unit-testing support is published on NuGet as [ThePensionsRegulator.Umbraco.Testing](https://www.nuget.org/packages/ThePensionsRegulator.Umbraco.Testing)

![ThePensionsRegulator.Umbraco.Testing on nuget.org](https://img.shields.io/nuget/vpre/ThePensionsRegulator.Umbraco.Testing)

## Using the GOV.UK Design System and TPR components in Umbraco

- [Configure a new Umbraco project (GOV.UK)](docs/umbraco/new-umbraco-project-govuk.md)
- [Configure a new Umbraco project (TPR)](docs/umbraco/new-umbraco-project-tpr.md)
- [Populate initial values](docs/umbraco/populate-initial-values.md)
- [Validation](docs/umbraco/validation.md)
- [Change how blocks are rendered](docs/umbraco/block-rendering.md)
- [Support full-width content](docs/umbraco/full-width-content.md)
- [Use SASS for CSS](docs/aspnet/sass.md)
- [Configure the rich text editor](/docs/umbraco/rich-text-editor.md)
- [Configure heading levels](/docs/umbraco/configure-heading-levels.md)

Umbraco GOV.UK Design System support is published on NuGet as [ThePensionsRegulator.GovUk.Frontend.Umbraco](https://www.nuget.org/packages/ThePensionsRegulator.GovUk.Frontend.Umbraco)

![ThePensionsRegulator.GovUk.Frontend.Umbraco on nuget.org](https://img.shields.io/nuget/vpre/ThePensionsRegulator.GovUk.Frontend.Umbraco)

Umbraco GOV.UK Design System support with TPR styling and components is published on NuGet as [ThePensionsRegulator.Frontend.Umbraco](https://www.nuget.org/packages/ThePensionsRegulator.Frontend.Umbraco)

![ThePensionsRegulator.Frontend.Umbraco on nuget.org](https://img.shields.io/nuget/vpre/ThePensionsRegulator.Frontend.Umbraco)

## Contributing to this project

This project prioritises the components required by The Pensions Regulator (TPR). Please open an issue if you find a bug, want to request improvements to a component we already support, or wish to implement a component we do not yet support. Pull requests are welcome.

We also encourage contributions to the base project we're building upon, [govuk-frontend-aspnetcore](https://github.com/gunndabad/govuk-frontend-aspnetcore).

See [Contributing to govuk-frontend-aspnetcore-extensions](CONTRIBUTING.md) for how to implement and test features.

## Disclaimer

This is a community implementation of the GOV.UK Design System. The Design System team is not responsible for it and cannot support you with using it. [Open an issue](https://github.com/thepensionsregulator/govuk-frontend-aspnetcore-extensions/issues) if you need help or you want to request a feature.
