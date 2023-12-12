# Checkboxes

For examples see [ASP.NET syntax for the Checkboxes component](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/checkboxes.md).

## Umbraco

You can add a checkboxes component to a block grid or block list in Umbraco. For examples of this component in use, see the 'Checkboxes' page in the Umbraco example app.

See [Validation](/docs/umbraco/validation.md) for how to validate a checkboxes component.

### Create checkboxes at runtime

You can configure a fixed set of checkboxes in the Umbraco backoffice, or you can supply checkboxes at runtime from a database or other data source.

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

        var block = viewModel.Page.Blocks.FindBlockByContentTypeAlias(GovukCheckboxes.ModelTypeAlias);
        block.Content.OverrideCheckboxes(new CheckboxItemBase[] {
                new Checkbox("1", "Item 1"),
                new Checkbox ("2", "Item 2"),
                new CheckboxesDivider(),
                new Checkbox("3", "Item 3")
            }, _publishedSnapshotAccessor);

        return CurrentTemplate(viewModel);
    }
}
```

### Pre-select checkboxes

On initial page load you can pre-select one or more checkboxes by setting the initial value(s) to match the value configured in the 'Checkbox' component in Umbraco.

For enum values, convert either the enum name or value to a string. If the string matches the value configured in the 'Checkbox' component in Umbraco it will be selected.

```csharp
// View model
public enum ExampleEnum
{
    Option1 = 1,
    Option2 = 2,
}

public class ExampleViewModel
{
    public List<string>? Field1 { get; set; }
    public List<ExampleEnum>? Field2 { get; set; }
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

        // Select a single checkbox
        ModelState.SetInitialValue(nameof(viewModel.Field1), "1");

        // Select multiple checkboxes
        ModelState.SetInitialValue(nameof(viewModel.Field1), new StringValues(new[] { "1", "2" }));

        // Select checkboxes from enum values, where the checkbox value matches the enum integer value
        var enums = new[] { ExampleEnum.Option1, ExampleEnum.Option2 };
        ModelState.SetInitialValue(nameof(viewModel.Field2), new StringValues(enums.Select(x => ((int)x).ToString()).ToArray()));

        // Select checkboxes from enum values, where the checkbox value matches the enum name
        ModelState.SetInitialValue(nameof(viewModel.Field2), new StringValues(enums.Select(x => x.ToString()).ToArray()));

        return CurrentTemplate(viewModel);
    }
}
```
