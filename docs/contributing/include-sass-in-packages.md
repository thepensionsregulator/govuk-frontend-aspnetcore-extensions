# Include SASS files in packages for consuming applications to use

SASS functions, mixins and variables are included in packages published by this solution. They are intended for use by consuming applications that want to use govuk-frontend or tpr-frontend without repeating values or calculations.

To package a SASS file for use by consuming applications, if the SASS file is in the `Styles` folder, set its build action to `Content`. If it is not in the `Styles` folder, include XML similar to the following example in the `*.csproj` file. Replace values in `{curly braces}`.

```xml
<Content Include="$(MsBuildThisFileDirectory)\{path}\{filename}.scss" Link="$(MsBuildThisFileDirectory)">
    <Pack>true</Pack>
    <PackagePath>contentFiles\any\net8.0\Styles\{folder}</PackagePath>
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
