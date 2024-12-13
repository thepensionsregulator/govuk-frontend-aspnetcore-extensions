# Configure a new Umbraco project (GOV.UK)

1. Run `dotnet new install Umbraco.Templates::13.5.2` to install Umbraco project templates. You must use Umbraco 13.x as we only support LTS versions of Umbraco.

2. Create a new project using the 'Umbraco project (Umbraco HQ)' template in Visual Studio, or by running `dotnet new umbraco --name MyProject`.

3. Add `ThePensionsRegulator.GovUk.Frontend.Umbraco` NuGet package to your project.

4. In your Umbraco project install the `uSync` NuGet package, making sure that the version aligns with the version of Umbraco you installed. See [uSync for Umbraco](https://jumoo.co.uk/usync/).

5. In `appsettings.json` add the following configuration. The settings shown for `Umbraco:CMS` are in addition to those present by default, not a replacement.

   ```json
   {
     "Umbraco": {
       "CMS": {
         "Global": {
           "UseHttps": true,
           "SanitizeTinyMce": true
         },
         "ModelsBuilder": {
           "ModelsMode": "SourceCodeManual",
           "ModelsDirectory": "~/Models/ModelsBuilder"
         },
         "RichTextEditor": {
           "ValidElements": "+a[id|rel|data-id|data-udi|rev|charset|hreflang|lang|tabindex|type|name|href|target|class],-strong/-b[class],-em/-i[class],-strike[class],p[id|class],-ol[style|class|reversed|start|type],-ul[class],-li[class],br[class],-sub[class],-sup[class],-blockquote[class],-table[class|id|lang],-tr[id|lang|class|rowspan],tbody[id|class],thead[id|class],tfoot[id|class],td[id|lang|class|colspan|rowspan|width],-th[id|lang|class|colspan|rowspan|width|scope],caption[id|lang|class],-div[id|class],-span[class],-pre[class],-h1[id|class],-h2[id|class],-h3[id|class],-h4[id|class],-h5[id|class],-h6[id|class],hr[class],small[class],dd[id|class|lang],dl[id|class|lang],dt[id|class|dir|lang]",
           "CustomConfig": {
             "table_advtab": "false",
             "table_cell_advtab": "false",
             "table_row_advtab": "false",
             "table_default_attributes": "{}",
             "table_default_styles": "{}",
             "table_class_list": "[{\"title\":\"None\",\"value\":\"\"},{\"title\": \"Width: three-quarters\",\"value\": \"govuk-!-width-three-quarters\"},{\"title\": \"Width: two-thirds\",\"value\": \"govuk-!-width-two-thirds\"},{\"title\": \"Width: one-half\",\"value\": \"govuk-!-width-one-half\"}]",
             "table_cell_class_list": "[{\"title\":\"None\",\"value\":\"\"},{\"title\": \"Numeric header cell\",\"value\": \"govuk-table__header--numeric\"},{\"title\": \"Numeric data cell\",\"value\": \"govuk-table__cell--numeric\"},{\"title\": \"Width: one-half\",\"value\": \"govuk-!-width-one-half\"},{\"title\": \"Width: one-third\",\"value\": \"govuk-!-width-one-third\"},{\"title\": \"Width: one-quarter\",\"value\": \"govuk-!-width-one-quarter\"}]"
           }
         },
         "RuntimeMinification": {
           "UseInMemoryCache": true,
           "CacheBuster": "Timestamp"
         }
       }
     }
   }
   ```

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
   @addTagHelper *, GovUk.Frontend.AspNetCore.Extensions
   ```

9. Create `Views/Shared/_Layout.cshtml` and include the GOV.UK Design System partial views and the `govuk-template__body` class. A minimal layout file looks like this:

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
       <partial name="GOVUK/UmbracoBodyClosing" />
     </body>
   </html>
   ```

10. Rebuild and run your project. On the first run you will see the 'Install Umbraco' screen. Enter your name, email and create a password when prompted.

    > If you get `BootFailedException: Boot failed: Umbraco cannot run.` the real error can be found in the `umbraco\Logs` folder. The most common error is `SQLite Error 14: 'unable to open database file'`. The most common fix for this is to delete the `ConnectionStrings:umbracoDbDSN` and `ConnectionStrings:umbracoDbDSN_ProviderName` settings from `appsettings.Development.json`. These will be re-instated on startup.

11. Go to the Umbraco back office at `/umbraco`. Navigate to Settings > uSync. In the 'Everything' box, click 'Import'.

12. In Settings > Document types select Create > Document Type with Template. In the Permissions settings for the document type, enable 'Allow as root'.

13. On the new document type, add a group and then add a property that uses the 'GOV.UK Block grid' or 'GOV.UK Block list' data type.

14. Go to Settings > Settings > Models Builder and click 'Generate Models'.

15. In Visual Studio you will find a new view for the document type in the `Views` folder. Add the following partial view, where `Model.Blocks` is the block grid or block list property you just created, as represented by the model generated by Umbraco Models Builder for the document type:

    ```razor
    <partial name="GOVUK/BlockGrid" model="Model.Blocks" />
    ```

    or

    ```razor
    <partial name="GOVUK/BlockList" model="Model.Blocks" />
    ```

    Remove `Layout = null;` from the top of the default template/view.

    Later, when you add form components to your page, you will need to create a `<alias>SurfaceController` class (where `<alias>` is the alias of your document type) and update your view to look more like this:

    ```razor
    @using (Html.BeginUmbracoForm<HomeSurfaceController>(nameof(HomeSurfaceController.Index), new {}, new { novalidate="novalidate" }))
    {
      <partial name="GOVUK/BlockGrid" model="Model.Blocks" />
    }
    ```

16. In `Program.cs` add the following to the `ConfigureServices` method:

    ```csharp
    using GovUk.Frontend.Umbraco;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Umbraco.Cms.Core.Models.PublishedContent;
    using Umbraco.Cms.Core.Web;

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    builder.Services.AddGovUkFrontendUmbraco();

    // default code from `builder.CreateUmbracoBuilder()` down to `app.UseUmbraco()` goes here...

    var mvcOptions = app.Services.GetRequiredService<IOptions<MvcOptions>>();
    var umbracoContextAccessor = app.Services.GetRequiredService<IUmbracoContextAccessor>();
    var publishedValueFallback = app.Services.GetRequiredService<IPublishedValueFallback>();
    app.UseGovUkFrontendUmbraco(mvcOptions, umbracoContextAccessor, publishedValueFallback);

    // await app.RunAsync(); goes here...
    ```
