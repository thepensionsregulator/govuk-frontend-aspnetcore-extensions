# Use GOV.UK and TPR styles in the Umbraco backoffice

The Umbraco backoffice can use our own styles so that editors work with components that look more like the finished page. However, it has some requirements:

- CSS files must be on disk, they cannot be embedded in a .NET DLL
- For the rich text editor, CSS files must be in the `wwwroot\css` folder.
- For the 'Formats' dropdown in the rich text editor, we need to add extra classes and consuming projects may also want to add classes.

We package one CSS file intended for the Umbraco backoffice, `govuk-umbraco-backoffice.css`. Similar to the process for SASS files above, this is added to the `contentFiles\any\net8.0\Content\css` folder in the NuGet package by XML added to the `*.csproj` file. A `<package-name>.targets` file is added to the `build` and `buildTransitive` folders in the package. The `<package-name>.targets` file finds the path to the package and the `Content\css` folder from there to `wwwroot\css` inside the consuming project. It generates a `.gitignore` file in that folder to prevent `govuk-umbraco-backoffice.css` being committed with the consuming application.

The `ThePensionsRegulator.Frontend.Umbraco` package deliberately overwrites `govuk-umbraco-backoffice.css` with its own version rather than using a different filename. This means the backoffice consistently uses either GOV.UK or TPR styling without needing a separate TPR element type for every GOV.UK component that needs a rich text editor.

`ThePensionsRegulator.GovUk.Frontend.Umbraco.targets` generates a `wwwroot\css\site.css` file if it's not present. Consuming applications can create their own `site.css` file, in which case we do not generate one. This is because rich text editor data types are configured to look for `site.css`. If it's not present the backoffice generates a request with a 404 response, so the generated file is there to respond to that request.
