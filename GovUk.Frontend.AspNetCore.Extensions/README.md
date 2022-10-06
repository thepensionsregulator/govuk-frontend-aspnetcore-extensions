# GovUk.Frontend.AspNetCore.Extensions

This builds on [ASP.NET Core MVC tag helpers for GOV.UK Design System](https://github.com/gunndabad/govuk-frontend-aspnetcore) by James Gunn, adding support for:

- ASP.NET client-side validation using [jQuery Unobtrusive Validation](https://github.com/aspnet/jquery-validation-unobtrusive)

JQuery is included to support the standard ASP.NET validation. We recommend using vanilla JavaScript for everything else.

This repository includes an example application which demonstrates the validation working both client-side and server-side.

## Getting started

1. Clone this repo

2. Clone the `govuk-frontend` submodule:

   ```pwsh
   git submodule update --init
   ```

3. Add `GovUk.Frontend.AspNetCore.Extensions` as a project reference. (This will be available on NuGet later.)

4. In `Startup.cs` add the following to the `ConfigureServices` method:

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

5. Add partial views and the `govuk-template__body` class to your layout file as shown below. You should also make sure you have a `<main>` element in your markup.

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

6. Add the following to your `Views/_ViewImports.cshtml` file:

   ```csharp
   @addTagHelper *, GovUk.Frontend.AspNetCore
   @addTagHelper *, GovUk.Frontend.AspNetCore.Extensions
   ```

7. [Add validation rules to your model](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/validation?view=aspnetcore-5.0) as you normally would for ASP.NET, using attributes from the [System.ComponentModel.DataAnnotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-5.0) namespace.

8. Add components from the GOV.UK Design System as documented in [ASP.NET Core MVC tag helpers for GOV.UK Design System](https://github.com/gunndabad/govuk-frontend-aspnetcore), but with wrapper tags from `GovUk.Frontend.AspNetCore.Extensions`.

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

## Custom Validation Attributes

To implement Custom Validation Attributes _and_ Client Side Validation, you need to do the following:

1. Create a validation attribute that inherits from `ValidationAttribute`. This attribute can take additional parameters, but should override `IsValid`.

```csharp
[AttributeUsage(AttributeTargets.Property)]
public class BananaValidatorAttribute : ValidationAttribute
{
    private string AdditionalParameter { get; set; }

    public BananaValidatorAttribute(string additionalParameter)
    {
        AdditionalParameter = additionalParameter;
    }

    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        // Do some validation here
        return ValidationResult.Success;
    }
}
```

2. Create an Attribute Adapter - this should inherit from ```AttributeAdapterBase<T>``` and needs to be able to pass a localiser to the base class, and override ```AddValidation```


```csharp
    public class BananaValidatorAttributeAdapter : AttributeAdapterBase<BananaValidatorAttribute>
    {
        public BananaValidatorAttributeAdapter(BananaValidatorAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer) { }

        public override void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
        var errorMessage = GetErrorMessage(context);
        MergeAttribute(context.Attributes, "data-val-banana", errorMessage);
        MergeAttribute(context.Attributes, "data-val-banana-additionalParameter", AdditionalParameter);
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
        }
    }

```

3. Create an Attribute Adapter Provider - you only need one, but you can add multiple Custom Validation Attribute Adapters as you need to:

```csharp
public class CustomValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute is BananaValidatorAttribute)
                return new BananaValidatorAttributeAdapter(attribute as BananaValidatorAttribute, stringLocalizer);
            else
                return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
```

4. Register this Adapter Provider in ```Startup.cs```

```csharp
services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidationAttributeAdapterProvider>();
```

5. Apply your attribute to the model as per normal

```csharp
[BananaValidator("My Additional Attribute", ErrorMessage = "An error message")]
public string Field { get; set; }
```

6. In the `.cshtml` file that uses the model, you will need to write a client-side validator that hooks into jquery validate, and can be set up with unobtrusive.

```javascript
<partial name="GOVUK/Validation" />
<script type="text/javascript">
    const govuk = createGovUkValidator();
    const validator = govuk.getValidator();
    govuk.createErrorSummary();

    if (validator) {
        validator.setDefaults({
            highlight: govuk.showError,
            unhighlight: govuk.removeOrUpdateError,
        });

        validator.addMethod('banana', function (value, element, params) {
            var additionalParameter = params.additionalParameter;
            // Do some validation here            
            return true;
        });

        validator.unobtrusive.adapters.add("banana", ["additionalParameter"],
            function (options) {
                options.rules['banana'] = options.params;
                options.messages['banana'] = options.message;
            }
        );
        validator.unobtrusive.parse();
    }
</script>
```

7. All being well, you should find that the generated html element now has `data-val-banana` and `data-val-banana-additionalparamter` attributes. These correspond to the validation attribute added to the view model.

8. Client-side validation is handled by `$.validator`, which should now have a `banana` method that is called by the govuk-validation.js code.

## Shared Resource Strings

In larger projects, it is common to have multiple projects with View-Models that share fields - for example, Email, Address, Phone Numbers etc. Each of these fields will have the same validation error message - for example, an Email Address will always need the same "Please enter a valid email address" message

Rather than replicate the same error message across all projects that have "Email Address" attribute, a better approach would be to have a shared resource

By default, View-Model Data Annotation error messages are found within a ```Resources``` folder: assuming the model class exists in ```ViewModels/MyModel.cs```, a typical resource file would be ```Resources/ViewModels/MyModel.de.resx```

The scaffolding for this is:

```csharp
    services.AddMvc()
        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
        .AddDataAnnotationsLocalization();
```

The _downside_ to this is that it only supports one resource file per model. For example, if a project contains multiple address fields, the same error message will need to be copied throughout the project.

One solution to this is to use a custom Localizer that allows for a (graceful) fallback to some other resource file. This localizer is part of the ```GovUk.Frontend.AspNetCore.Extensions``` library, called ```DataAnnotationStringLocalizer```

Setup is simple

1. Create an empty class (for example ```SharedResource.cs```) in the root of your project - this can be a shared project elsewhere
2. Add a ```Resources``` folder in the same project, and create resource files with the same name as your empty class - for example ```SharedResource.es.resx```, ```SharedResource.fr.resx```
3. In ```Startup.cs```, instead of the default ```AddDataAnnotationsLocalization```, we use ```DataAnnotationStringLocalizer``` and provide SharedResource as a fallback.

```csharp
services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(o =>
    {
        o.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName!);

            return new DataAnnotationStringLocalizer(
                factory?.Create(type),
                factory?.Create(nameof(SharedResource), assemblyName.Name!)
            );
        };
    });
```

4. Attribute error messages are then searched for by the local project Resource folder (e.g. ```Resources/ViewModels/MyModel.de.resx```), and then (if nothing is found) the referenced class - in the example above, ```SharedResource```
