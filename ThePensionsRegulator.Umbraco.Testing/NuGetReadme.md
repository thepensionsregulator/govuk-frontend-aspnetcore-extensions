# ThePensionsRegulator.Umbraco.Testing

Helper code for unit testing Umbraco applications, and in particular applications built using [ThePensionsRegulator.Umbraco](https://www.nuget.org/packages/ThePensionsRegulator.Umbraco).

Creating an `UmbracoTestContext` will give you an Umbraco context that mocks a page request using [Moq](https://github.com/moq/moq4), including mocks of Umbraco and .NET services used in that page request.

Factory and extension methods make it easy to set up mocks of common property types on the page, including block lists and block grids.

See the [documentation on Github](https://github.com/thepensionsregulator/govuk-frontend-aspnetcore-extensions/blob/develop/docs/umbraco/unit-testing.md) for further details.
