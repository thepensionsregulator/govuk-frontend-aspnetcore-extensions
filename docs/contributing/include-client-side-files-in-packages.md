# Include client-side files in packages

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
