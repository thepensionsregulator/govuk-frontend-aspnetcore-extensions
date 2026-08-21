# Configure a new Umbraco project (GOV.UK)

1. Run `dotnet new install Umbraco.Templates::17.0.2` to install Umbraco project templates. You must use Umbraco 17.x as we only support LTS versions of Umbraco.

2. Create a new project using the 'Umbraco project (Umbraco HQ)' template in Visual Studio 2026 or later, or by running `dotnet new umbraco --name MyProject`. Select .NET 10.0 or later as the Framework (or .NET 9 if .NET 10 is not shown).

3. Add `ThePensionsRegulator.GovUk.Frontend.Umbraco` NuGet package to your project.

4. In your Umbraco project install the `uSync` NuGet package, making sure that the version aligns with the version of Umbraco you installed. See [uSync for Umbraco](https://jumoo.co.uk/usync/).

5. Update `appsettings.json` and `appsettings.Development.json` with the [recommended appsettings configuration for a new Umbraco project](new-umbraco-project-appsettings.md).

6. Create a `wwwroot/media` folder with the following `.gitignore` file in it.

   ```text
   *
   !.gitignore
   ```

7. Create `Views/_ViewStart.cshtml` with the following content:

   ```razor
   @{
       Layout = "_Layout";
   }
   ```

8. Add the following to your `Views/_ViewImports.cshtml` file:

   ```razor
   @addTagHelper *, GovUk.Frontend.AspNetCore
   @addTagHelper *, ThePensionsRegulator.GovUk.Frontend
   ```

9. Create `Views/Shared/_Layout.cshtml` with the code shown below.

   ```razor
   @inherits Umbraco.Cms.Web.Common.Views.UmbracoViewPage<object>
   <!DOCTYPE html>
   <html lang="en">
     <head>
       <meta charset="utf-8" />
       <meta name="viewport" content="width=device-width, initial-scale=1.0" />
       <partial name="GOVUK/UmbracoHead" />
       <title>@Umbraco.AssignedContentItem.Name</title>
       @RenderSection("head", required: false)
     </head>
     <body class="govuk-template__body ">
       <partial name="GOVUK/BodyOpen" />
       <div class="govuk-width-container">
         <main class="govuk-main-wrapper" id="main">@RenderBody()</main>
       </div>
       <partial name="GOVUK/BodyClosing" />
       @RenderSection("scripts", required: false)
     </body>
   </html>
   ```

10. Build and run your project. On the first run you will see the 'Install Umbraco' screen. Enter your name, email and create a password when prompted. Accept the defaults for other settings.

    > If you get `BootFailedException: Boot failed: Umbraco cannot run.` the real error can be found in the `umbraco\Logs` folder. The most common error is `SQLite Error 14: 'unable to open database file'`. The most common fix for this is to delete the `ConnectionStrings:umbracoDbDSN` and `ConnectionStrings:umbracoDbDSN_ProviderName` settings from `appsettings.json`. These will be re-instated when you run the application.

11. Go to the Umbraco back office at `/umbraco`. Navigate to Settings > uSync. In the 'Everything' box, click 'Import'.

12. In Settings > Document types select Create > Document Type with Template. Call it 'Home'. In the Structure workspace for the new document type, enable 'Allow at root'.

13. In the Design workspace for the new document type, add a group and then add a property that uses the 'GOV.UK Block grid' or 'GOV.UK Block list' property editor. Save the document type.

14. Click the Settings section again in the header, then 'Models Builder' and finally 'Generate Models'.

15. In Visual Studio you will find a new view for the document type in the `Views` folder.

    Add the generated model for your document type to the `@inherits` statement. For example, replace this:

    `@inherits Umbraco.Cms.Web.Common.Views.UmbracoViewPage`

    with this:

    `@inherits Umbraco.Cms.Web.Common.Views.UmbracoViewPage<Home>`

    Remove `Layout = null;` from the top of the view.

    Add the following partial view, where `Model.Blocks` is the block grid or block list property you just added:

    ```razor
    <partial name="GOVUK/BlockGrid" model="Model.Blocks" />
    ```

    or

    ```razor
    <partial name="GOVUK/BlockList" model="Model.Blocks" />
    ```

    Later, when you add form components to your page, you will need to create an `<alias>SurfaceController` class (where `<alias>` is the alias of your document type) and update your view to look more like this:

    ```razor
    @using (Html.BeginUmbracoForm<HomeSurfaceController>(nameof(HomeSurfaceController.Index), new {}, new { novalidate="novalidate" }))
    {
      <partial name="GOVUK/BlockGrid" model="Model.Blocks" />
    }
    @section scripts {
      <partial name="GOVUK/Validation" />
    }
    ```

16. In `Program.cs` add the following:

    ```csharp
    using ThePensionsRegulator.GovUk.Frontend.Umbraco;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Umbraco.Cms.Core.Models.PublishedContent;
    using Umbraco.Cms.Core.Web;

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    builder.Services.AddTprGovUkFrontendUmbraco();

    // builder.CreateUmbracoBuilder() goes here

    WebApplication app = builder.Build();
    var mvcOptions = app.Services.GetRequiredService<IOptions<MvcOptions>>();
    var umbracoContextAccessor = app.Services.GetRequiredService<IUmbracoContextAccessor>();
    var publishedValueFallback = app.Services.GetRequiredService<IPublishedValueFallback>();
    app.UseTprGovUkFrontendUmbraco(mvcOptions, umbracoContextAccessor, publishedValueFallback);

    // await app.BootUmbracoAsync() and app.UseUmbraco() go here

    await app.RunAsync();
    ```

17. Re-run your application and you should now be able to create a Home page and add GOV.UK content blocks to it.
