# Validation

> JQuery is included to support the standard ASP.NET validation. We recommend using vanilla JavaScript for everything else.

To use validation, in `Startup.cs` add the following to the `Configure` method:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Web;

public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<MvcOptions> mvcOptions, IUmbracoContextAccessor umbracoContextAccessor)
{
    // Other code here, including app.UseUmbraco()...

    // Note: two extra services are being injected to the Configure method and used here
    app.UseGovUkFrontendUmbracoExtensions(mvcOptions, umbracoContextAccessor);
}
```

On your controller and surface controller, add a `ModelType` attribute identifying the type of your view model.

```csharp
using GovUk.Frontend.AspNetCore.Extensions.Validation;

public class MyDocumentTypeController : RenderController
{
    private readonly IVariationContextAccessor _variationContextAccessor;
    private readonly ServiceContext _serviceContext;

    public MyDocumentTypeController(ILogger<MyDocumentTypeController> logger,
      ICompositeViewEngine compositeViewEngine,
      IUmbracoContextAccessor umbracoContextAccessor,
      IVariationContextAccessor variationContextAccessor,
      ServiceContext serviceContext) : base(logger, compositeViewEngine, umbracoContextAccessor)
    {
      _variationContextAccessor = variationContextAccessor;
      _serviceContext = serviceContext;
    }

    [ModelType(typeof(MyDocumentType))]
    public override IActionResult Index()
    {
      return CurrentTemplate(new MyDocumentType(CurrentPage, new PublishedValueFallback(_serviceContext, _variationContextAccessor)));
    }
}
```

In your shared layout, or just the views where you need it, add the following partial view just before the closing `body` tag.

```html
<partial name="GOVUK/UmbracoValidation" />
```

[Add validation rules to your model](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/validation?view=aspnetcore-5.0) as you normally would for ASP.NET, using attributes from the [System.ComponentModel.DataAnnotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-5.0) namespace.

You will need a custom view model for this. Add the model generated by Models Builder as a property named `Page` so that it can still be used in the view. For the error message use the name of the property you're applying validation to. In the Umbraco back office go to the settings of the block (for example, a 'Text input'), and select the property from the view model in the 'Model property' setting. This allows `GovUk.Frontend.Umbraco` to hook the view model property to the Umbraco content and replace the error message with an error message configured in the Umbraco back office.

```csharp
public class MyDocumentTypeViewModel
{
    public MyDocumentType? Page { get; set; }

    [Required(ErrorMessage = nameof(PropertyToValidate))]
    public int? PropertyToValidate { get; set; }
}
```

If the standard validation support does not meet your needs you can add an 'Error message' block in the block list editor. Select a view model property in the settings for the 'Error message' block, and it will appear only when that property is invalid in `ModelState`.

## Validate multiple fields together

If you need to validate several fields together, place the fields inside a 'Fieldset' block. Add a property on your view model which calculates and validates a value derived from all the relevant fields. (If your requirement is more complex you could build an instance of a class and apply a [custom validation attribute](<https://learn.microsoft.com/en-us/previous-versions/aspnet/cc668224(v=vs.100)>).)

```csharp
public class MyDocumentTypeViewModel
{
    public MyDocumentType? Page { get; set; }

    public int? Field1 { get; set; }

    public int? Field2 { get; set; }

    [Range(100, 100, ErrorMessage = nameof(AddsUpTo100))]
    public int? AddsUpTo100 { get => Field1 + Field2; }
}
```

Add an 'Error message' block directly inside the 'Fieldset' block, and on the settings for the 'Error message' block select the calculated property as the 'Model property'. When this property is invalid in `ModelState` the whole fieldset will be invalid.

If you need an 'Error message' block inside a 'Fieldset' block without this behaviour, you can disable it on the settings of the 'Fieldset' block.

## Validating UK postcodes, Companies House company numbers and registered charity numbers

See the [Text input](../components/text-input.md) component for details of validators included with the `ThePensionsRegulator.GovUk.Frontend` package.
