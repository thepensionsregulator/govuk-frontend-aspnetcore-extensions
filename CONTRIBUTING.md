# Contributing to govuk-frontend-aspnetcore-extensions

This project prioritises the components required by The Pensions Regulator (TPR). Please open an issue if you find a bug, want to request improvements to a component we already support, or wish to implement a component we do not yet support. Pull requests are welcome.

We also encourage contributions to the base project we're building upon, [govuk-frontend-aspnetcore](https://github.com/gunndabad/govuk-frontend-aspnetcore).

## Update the versions of govuk-frontend-aspnetcore and GOV.UK Frontend

This project builds on [govuk-frontend-aspnetcore](https://github.com/gunndabad/govuk-frontend-aspnetcore) by James Gunn, which in turn builds on [GOV.UK Frontend](https://github.com/alphagov/govuk-frontend).

When you update either you must:

- update `govuk-frontend-aspnetcore` in both the `GovUk.Frontend.AspNetCore.Extensions` and `ThePensionsRegulator.Frontend` packages.
- check the release notes for both projects for changes that we may need to implement
- update the version of GOV.UK Frontend in [README.md](README.md)
- update the GOV.UK Frontend submodule (see below)

This project references GOV.UK Frontend as a submodule, so that we can import and use the SASS code. This reference **must** be kept aligned with the version referenced by `govuk-frontend-aspnetcore`, therefore any time you update the `govuk-frontend-aspnetcore` NuGet package you should check the release notes to see whether you need to update GOV.UK Frontend.

This example shows how to update the reference to GOV.UK Frontend.

```cmd
cd lib\govuk-frontend
git pull
git checkout v4.3.0
cd ..\..
git commit -am "Update govuk-frontend to v4.3.0"
```

If you've worked on this project before it's possible that someone else has updated the version of GOV.UK Frontend in the meantime. You should run `git submodule update` periodically to ensure you stay up-to-date.

## Package SASS files for consuming applications to use

SASS functions, mixins and variables are included in packages published by this solution. They are intended for use by consuming applications that want to use govuk-frontend or tpr-frontend without repeating values or calculations.

To package a SASS file for use by consuming applications, if the SASS file is in the `Styles` folder, set its build action to `Content`. If it is not in the `Styles` folder, include XML similar to the following example in the `*.csproj` file. Replace values in `{curly braces}`.

```xml
<Content Include="$(MsBuildThisFileDirectory)\{path}\{filename}.scss" Link="$(MsBuildThisFileDirectory)">
    <Pack>true</Pack>
    <PackagePath>contentFiles\any\net6.0\Styles\{folder}</PackagePath>
</Content>
```

This creates a reference in the consuming project back to the file in the package folder, but the file isn't physically in the project folder which makes it difficult to reference from another SASS file. To solve this, create a `<package-name>.targets` file and add it to the `build` and `buildTransitive` folders in the package using XML similar to the following example in the `*.csproj` file. Replace values in `{curly braces}`.

```xml
<Content Include="{package-name}.targets">
    <Pack>true</Pack>
    <PackagePath>build;buildTransitive</PackagePath>
</Content>
```

The `<package-name>.targets` file should find the path to the package and copy the SASS files from there to a subfolder of the `Styles` folder inside the consuming project. It should generate a `.gitignore` file in that folder to prevent the files being committed with the consuming application. See `ThePensionsRegulator.GovUk.Frontend.targets` for an example.

## Use GOV.UK and TPR styles in the Umbraco backoffice

The Umbraco backoffice can use our own styles so that editors work with components that look more like the finished page. However, it has some requirements:

- CSS files must be on disk, they cannot be embedded in a .NET DLL
- For the rich text editor, CSS files must be in the `wwwroot\css` folder.
- For the 'Formats' dropdown in the rich text editor, we need to add extra classes and consuming projects may also want to add classes.

We package one CSS file intended for the Umbraco backoffice, `govuk-umbraco-backoffice.css`. Similar to the process for SASS files above, this is added to the `contentFiles\any\net6.0\Content\css` folder in the NuGet package by XML added to the `*.csproj` file. A `<package-name>.targets` file is added to the `build` and `buildTransitive` folders in the package. The `<package-name>.targets` file finds the path to the package and the `Content\css` folder from there to `wwwroot\css` inside the consuming project. It generates a `.gitignore` file in that folder to prevent `govuk-umbraco-backoffice.css` being committed with the consuming application.

The `ThePensionsRegulator.Frontend.Umbraco` package deliberately overwrites `govuk-umbraco-backoffice.css` with its own version rather than using a different filename. This means the backoffice consistently uses either GOV.UK or TPR styling without needing a separate TPR element type for every GOV.UK component that needs a rich text editor.

`ThePensionsRegulator.GovUk.Frontend.Umbraco.targets` generates a `wwwroot\css\site.css` file if it's not present. Consuming applications can create their own `site.css` file, in which case we do not generate one. This is because rich text editor data types are configured to look for `site.css`. If it's not present the backoffice generates a request with a 404 response, so the generated file is there to respond to that request.

## Embed client-side files to be requested by the browser

CSS, JavaScript, image and font files that just need to be requested by the browser can be embedded in the NuGet package just by placing them in the `wwwroot\govuk` or `wwwroot\tpr` folder of your project. They will be available as `/_content/<package-name>/<path-within-wwwroot>`.

CSS files can be created directly within `wwwroot\govuk` or `wwwroot\tpr`, or they can be generated from a SASS file in the `Styles` folder. When generating a CSS file from SASS exclude the generated file from source control. You will need to add a command in `Directory.Build.targets` to compile the SASS to CSS.

If you need to change the path to a well-known path, for example `/favicon.ico` instead of `/_content/ThePensionsRegulator.Frontend/favicon.ico`, you can create a `<package-name>.props` file.

Look inside the NuGet package for the default `build\Microsoft.AspNetCore.StaticWebAssets.props` file and copy the syntax for the file you want to update into a `<package-name>.props` file. Change the `BasePath` content to the path you want instead of `/_content/<package-name>`. `<package-name>.props` must include the following import directive as the first element inside `<Project>`:

```xml
<Project>
    <Import Project="../build/Microsoft.AspNetCore.StaticWebAssets.props" />
    ...your changes...
</Project>
```

Update the `*.csproj` file to add the `<package-name>.props` file to the package in the `build` and `buildTransitive` folders. Replace values in `{curly braces}`.

```xml
<Content Include="{package-name}.props">
    <Pack>true</Pack>
    <PackagePath>build;buildTransitive</PackagePath>
</Content>
```

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

## Publish a new version to nuget.org

Tag the commit you want to publish from. Name your tag using semantic versioning. For example, if you are upgrading all packages from v1.0.0 to v2.0.0, your tag should be `v2.0.0`. If you are upgrading only one package then append the package name - for example `v2.0.0-ThePensionsRegulator.GovUk.Frontend`.

To publish to nuget.org, run `azure-pipelines.yml` based on the tag.

Add details of the release to the [Releases](https://github.com/thepensionsregulator/govuk-frontend-aspnetcore-extensions/releases) section on Github.
