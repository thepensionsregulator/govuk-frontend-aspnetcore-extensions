# Radios

For examples see [ASP.NET syntax for the Radios component](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/radios.md).

## Umbraco

You can add a radios component to a block grid or block list in Umbraco. For examples of this component in use, see the 'Radios' page in the Umbraco example app.

See [Validation](/docs/umbraco/validation.md) for how to validate a radios component.

### Create radio buttons at runtime

You can configure a fixed set of radio buttons in the Umbraco backoffice, or you can supply radio buttons at runtime from a database or other data source.

```csharp
using ThePensionsRegulator.Umbraco.BlockLists;
using GovUk.Frontend.Umbraco.BlockLists;
using GovUk.Frontend.Umbraco.Models;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Web.Common.PublishedModels;

public class ExampleController : RenderController
{
    private readonly IPublishedValueFallback _publishedValueFallback;
    private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;

    public ExampleController(ILogger<RenderController> logger,
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor,
        IPublishedValueFallback publishedValueFallback,
        IPublishedSnapshotAccessor publishedSnapshotAccessor
        ) : base(logger, compositeViewEngine, umbracoContextAccessor)
    {
        _publishedValueFallback = publishedValueFallback;
        _publishedSnapshotAccessor = publishedSnapshotAccessor;
    }

    [ModelType(typeof(ExampleViewModel))]
    public override IActionResult Index()
    {
        var viewModel = new ExampleViewModel
        {
            Page = new ExampleModelsBuilderModel(CurrentPage, _publishedValueFallback)
        };

        var block = viewModel.Page.Blocks.FindBlockByContentTypeAlias(GovukRadios.ModelTypeAlias);
        block.Content.OverrideRadioButtons(new RadioItemBase[] {
                new RadioButton("1", "Item 1"),
                new RadioButton ("2", "Item 2"),
                new RadiosDivider(),
                new RadioButton("3", "Item 3")
            }, _publishedSnapshotAccessor);

        return CurrentTemplate(viewModel);
    }
}
```

### Pre-select a radio button

On initial page load you can pre-select a radio button by setting the initial value to match the value configured in the 'Radio' component in Umbraco.

For an enum value, convert either the enum name or value to a string. If the string matches the value configured in the 'Radio' component in Umbraco it will be selected.

```csharp
// View model
public enum ExampleEnum
{
    Option1 = 1,
    Option2 = 2,
}

public class ExampleViewModel
{
    public string? Field1 { get; set; }
    public ExampleEnum? Field2 { get; set; }
}
```

```csharp
// Controller
using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Umbraco.Cms.Web.Common.Controllers;

public class ExampleController : RenderController
{
    // …more controller code here

    [ModelType(typeof(ExampleViewModel))]
    public override IActionResult Index()
    {
        var viewModel = new ExampleViewModel();

        // Select a radio button by value
        ModelState.SetInitialValue(nameof(viewModel.Field1), "1");

        // Select a radio button where its value matches an enum integer value
        ModelState.SetInitialValue(nameof(viewModel.Field2), ((int)ExampleEnum.Option1).ToString());

        // Select a radio button where its value matches an enum name
        ModelState.SetInitialValue(nameof(viewModel.Field2), ExampleEnum.Option1.ToString());

        return CurrentTemplate(viewModel);
    }
}
```
