# Contributing to govuk-frontend-aspnetcore-extensions

This project prioritises the components required by The Pensions Regulator. Please open an issue if you find a bug, want to request improvements to a component we already support, or wish to implement a component we do not yet support. Pull requests are welcome.

We also encourage contributions to the base project we're building upon, [govuk-frontend-aspnetcore](https://github.com/gunndabad/govuk-frontend-aspnetcore).

## Update the version of GOV.UK Frontend

This project builds on [govuk-frontend-aspnetcore](https://github.com/gunndabad/govuk-frontend-aspnetcore) by James Gunn, which in turn builds on [GOV.UK Frontend](https://github.com/alphagov/govuk-frontend).

This project references GOV.UK Frontend as a submodule, so that we can import and use the SASS code. This reference **must** be kept aligned with the version referenced by `govuk-frontend-aspnetcore`, therefore any time you update the `govuk-frontend-aspnetcore` NuGet package you should check the release notes to see whether you need to update GOV.UK Frontend.

This example shows how to update the reference to GOV.UK Frontend:

```cmd
cd lib\govuk-frontend
git pull
git checkout v4.3.0
cd ..\..
git commit -am "Update govuk-frontend to v4.3.0"
```

You also need to update the path to the GOV.UK Frontend JavaScript in `BodyClosing.cshtml` (GOV.UK and TPR versions) `ApplicationBuilderExtensions.cs` (GOV.UK and TPR versions), and [README.md](README.md).

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
