# Use GOV.UK and TPR styles in the Umbraco backoffice

The Umbraco backoffice can use our own styles so that editors work with components that look more like the finished page. However, it has some requirements:

- CSS files must be on disk, they cannot be embedded in a .NET DLL
- For the rich text editor, CSS files must be in the `wwwroot\css` folder.
- For the 'Formats' dropdown in the rich text editor (when using TinyMCE rather than TipTap), we need to add extra classes and consuming projects may also want to add classes.

We package one CSS file intended for the Umbraco backoffice, `govuk-umbraco-backoffice.css`. Similar to other [client-side files in packages](include-client-side-files-in-packages.md), this is a SASS file in the `Styles` folder which generates `wwwroot\<package-name>\css\govuk-umbraco-backoffice.css`. Files under `wwwroot` are automatically included in the package.

However, we need the file to be under `wwwroot\css`. We could just generate it there, but we have two versions (GOV.UK and TPR) and if we package both at the same location we get a conflict. Instead we use the `<package-name>.targets` file that is added to the `build` and `buildTransitive` folders in each package. The `<package-name>.targets` file finds the path to the package and copies the file from the static web assets in the package to `wwwroot\css` inside the consuming project. It generates a `.gitignore` file in that folder to prevent `govuk-umbraco-backoffice.css` being committed with the consuming application.

`ThePensionsRegulator.Frontend.Umbraco.targets` runs after `ThePensionsRegulator.GovUk.Frontend.Umbraco.targets` and deliberately overwrites `govuk-umbraco-backoffice.css` with its own version rather than using a different filename. This means backoffice code for GOV.UK and TPR components can load styles from the same URL and consistently use either GOV.UK or TPR styling, depending which package is installed, without needing a separate TPR element type for every GOV.UK component that needs a rich text editor.

## Site-specific styles

Rich text editor data types are configured to look for `site.css`. Consuming applications can create their own `site.css` file, and the styles will automatically be imported into the rich text editor. If it's not present the backoffice generates a request with a 404 response, so `ThePensionsRegulator.GovUk.Frontend.Umbraco.targets` generates a `wwwroot\css\site.css` file to respond to that request if it doesn't already exist.
