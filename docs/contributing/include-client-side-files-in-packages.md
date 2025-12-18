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

## Minifying and caching client-side files

When including a client-side file on a page, always enable client-side caching with a cache-busting parameter that ensures the cache is reset when a new version of the package is published.

### JavaScript files

Configure minification in `bundleconfig.json` at the root of the project. This is processed by the `BuildBundlerMinifier` NuGet package. Then use `asp-append-version="true"` to add a cache-busting parameter:

```razor
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
<script src="/<package-name>/js/my-js-file.min.js" type="module" asp-append-version="true"></script>
```

### Add a Cache-Control header

If you add files to expected locations they will automatically get a `Cache-Control` header which caches them for a long time.

This is controlled by a class in each project which implements `ThePensionsRegulator.GovUk.Frontend.Caching.IStaticFileCachePolicy`. You can update the class for the project you're working on if you need the header added to other client-side paths.

`ThePensionsRegulator.GovUk.Frontend.Caching.CacheStaticFilesMiddleware` looks at all the `IStaticFileCachePolicy` instances and, if the path of the request matches, it adds the `Cache-Control` header.
