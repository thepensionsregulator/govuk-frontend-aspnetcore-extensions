# GovUk.Frontend.AspNetCore.Extensions

This builds on [ASP.NET Core MVC tag helpers for GOV.UK Design System](https://github.com/gunndabad/govuk-frontend-aspnetcore) by James Gunn, adding support for:

- ASP.NET client-side validation using [jQuery Unobtrusive Validation](https://github.com/aspnet/jquery-validation-unobtrusive)

JQuery is included to support the standard ASP.NET validation. We recommend using vanilla JavaScript for everything else.

This repository includes an example application which demonstrates the validation working both client-side and server-side.

## Getting started

1. Add `GovUk.Frontend.AspNetCore.Extensions` and `govuk-frontend-aspnetcore` (`v1.0` branch) as project references. (These will be available on NuGet later.)

2. In `Startup.cs` add the following to the `ConfigureServices` method:

   ```csharp
   using GovUk.Frontend.AspNetCore.Extensions;

   public void ConfigureServices(IServiceCollection services)
   {
       // Other code here

       services.AddGovUkFrontendExtensions(options =>
       {
           // Avoid adding scripts which require 'unsafe-inline' in the content security policy
           options.AddImportsToHtml = false;
       });
   }
   ```

3. Add partial views and the `govuk-template__body` class to your layout file as shown below. You should also make sure you have a `<main>` element in your markup.

   ```html
   <!DOCTYPE html>
   <html lang="en">
     <head>
       <meta charset="utf-8" />
       <meta name="viewport" content="width=device-width, initial-scale=1.0" />
       <partial name="TPR/Head" />
       @RenderSection("head", required: false)
     </head>
     <body class="govuk-template__body ">
       <partial name="GOVUK/BodyOpen" />
       <main>@RenderBody()</main>
       <partial name="GOVUK/BodyClosing" />
       <partial name="GOVUK/Validation" />
     </body>
   </html>
   ```

   Note that `TPR/Head` imports TPR styles on top of the GOV.UK Design System. You can use the partial `GOVUK/Head` instead to use the GOV.UK Design System styles only.

4. Add the following to your `Views/_ViewImports.cshtml` file:

   ```csharp
   @addTagHelper *, GovUk.Frontend.AspNetCore
   @addTagHelper *, GovUk.Frontend.AspNetCore.Extensions
   ```

5. [Add validation rules to your model](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/validation?view=aspnetcore-5.0) as you normally would for ASP.NET, using attributes from the [System.ComponentModel.DataAnnotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-5.0) namespace.

6. Add components from the GOV.UK Design System as documented in [ASP.NET Core MVC tag helpers for GOV.UK Design System](https://github.com/gunndabad/govuk-frontend-aspnetcore), but with wrapper tags from `GovUk.Frontend.AspNetCore.Extensions`.

   ```csharp
   <partial name="GOVUK/ErrorSummary" />

   <h1 class="govuk-heading-l">My form</h1>

   <form asp-controller="Home" asp-action="Post" method="post" novalidate>
       <govuk-client-side-validation>
           <govuk-input asp-for="MyModelProperty">
               <govuk-input-label>Field label</govuk-input-label>
               <govuk-input-hint>This is the hint</govuk-input-hint>
               <govuk-input-error-message />
           </govuk-input>
       </govuk-client-side-validation>

       <govuk-button type="submit">Submit</govuk-button>
   </form>
   ```

## Localisation Support

ASP.NET Core supports Localization via `ViewLocalization` and `DataAnnotationsLocalization` amongst other ways.

If enabled, the extension library automatically generates localised strings for `System.ComponentModel.DataAnnotations` error messages. The scaffolding for this is as follows:

1. In Startup.cs, add localization to `ConfigureServices`

   ```csharp
   services.AddLocalization(p => p.ResourcesPath = "Resources");

   services.AddMvc()
     .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
     .AddDataAnnotationsLocalization();

   services.Configure<RequestLocalizationOptions>(options =>
   {
     var supportedCultures = new List<CultureInfo> {
       new CultureInfo("en"),
       new CultureInfo("de")
     };

     options.DefaultRequestCulture = new RequestCulture("en-GB");
     options.SupportedCultures = supportedCultures;
     options.SupportedUICultures = supportedCultures;
   });
   ```

   This code does three things:

   - Instructs the Localization service to look in the "Resources" folder.
   - Tells Resource lookup to use suffixes to distinguish different languages (eg `TextInputViewModel.es.resx`)
   - Tells the Resource lookup which languages it should support

2. Add the following to the `Configure` method:

   ```csharp
   var supportedCultures = new[] {
       new CultureInfo("en"),
       new CultureInfo("de")
   };

   app.UseRequestLocalization(new RequestLocalizationOptions
   {
       DefaultRequestCulture = new RequestCulture("en"),
       SupportedCultures = supportedCultures,
       SupportedUICultures = supportedCultures
   });
   ```

   This adds supported cultures to each request.

3. Decorate model attributes as normal. For example:

   ```csharp
   [Required(ErrorMessage = "Phone number is required")]
   public string PhoneNumber { get; set; }
   ```

4. To switch languages, set the `CookieRequestCultureProvider` with the desired locale string. For example, this can be done via a `BaseController:

   ```csharp
   public class BaseController : Controller
   {
       [HttpGet]
       public IActionResult SetLanguage(string culture, string returnUrl)
       {
           Response.Cookies.Append(
               CookieRequestCultureProvider.DefaultCookieName,
               CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
               new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
           );

           return LocalRedirect(returnUrl);
       }
   }
   ```

   and corresponding Language switching links - e.g. a Shared Component:

   ```csharp
   @using Microsoft.AspNetCore.Builder
   @using Microsoft.AspNetCore.Http.Features
   @using Microsoft.AspNetCore.Localization
   @using Microsoft.AspNetCore.Mvc.Localization
   @using Microsoft.Extensions.Options

   @inject IOptions<RequestLocalizationOptions> LocOptions

   @{
       var requestCulture = Context.Features.Get<IRequestCultureFeature>();
       var cultureItems = LocOptions.Value.SupportedUICultures
           .Select(c => (Value: c.Name, Text: c.NativeName ))
           .ToList();
       var returnUrl = string.IsNullOrEmpty(Context.Request.Path) ? "~/" : $"~{Context.Request.Path.Value}";
       var currentLanguage = requestCulture.RequestCulture.UICulture.Name;
   }

   <div id="language-switcher-container">
       <ul id="language-switcher" class="govuk-list">
            @foreach (var culture in cultureItems)
            {
                <li class="@((currentLanguage == culture.Value) ? "languageSelected" : "languageNotSelected")"><a class="govuk-link" asp-action="SetLanguage" asp-controller="Home" asp-route-returnUrl="@returnUrl" asp-route-culture="@culture.Value">@culture.Text</a></li>
            }
        </ul>
   </div>
   ```

5. To implement a different language, create a resx file inside a Resources folder: you _must_ make sure that the Resource folder path matches the Solution folder structure. For example, if a model class is `~/ViewModels/Home/LoginModel.cs`, the corresponding resx file should be at `~/Resources/ViewModels/Home/LoginModel.de.resx`
