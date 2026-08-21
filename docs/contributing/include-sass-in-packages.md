# Include SASS files in packages for consuming applications to use

SASS functions, mixins and variables are included in packages published by this solution. They are intended for use by consuming applications that want to use GOV.UK Frontend or `ThePensionsRegulator.Frontend` without repeating values or calculations.

To package a SASS file for use by consuming applications, include XML similar to the following example in the `*.csproj` file. Replace values in `{curly braces}`. (GOV.UK Frontend SASS files included in `ThePensionsRegulator.GovUk.Frontend` use a slightly different syntax because they come from outside the project root.)

```xml
<Content Include="Styles\{filename}.scss">
    <Pack>true</Pack>
    <PackagePath>Styles\{folder}</PackagePath>
</Content>
```

This creates a reference in the consuming project back to the file in the package folder, but the file isn't physically in the project folder which makes it difficult to reference from another SASS file. To solve this, use the `<package-name>.targets` file in the project. This is already added to the `build` and `buildTransitive` folders in the package using XML similar to the following example in the `*.csproj` file, and it runs each time the consuming project is built. The value in `{curly braces}` is the project name.

```xml
<Content Include="{package-name}.targets">
    <Pack>true</Pack>
    <PackagePath>build;buildTransitive</PackagePath>
</Content>
```

The `<package-name>.targets` file should find the path to the package and copy the SASS files from there to a subfolder of the `Styles` folder inside the consuming project. It should generate a `.gitignore` file in that folder to prevent the files being committed with the consuming application. See `ThePensionsRegulator.GovUk.Frontend.targets` for an example.
