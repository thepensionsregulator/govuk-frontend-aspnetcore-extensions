# Checkboxes

For examples see [ASP.NET syntax for the Checkboxes component](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/checkboxes.md).

## Umbraco

You can add a checkboxes component to a block list in Umbraco. For examples of this component in use, see the 'Checkboxes' page in the Umbraco example app.

See [Validation](/docs/umbraco/validation.md) for how to validate a checkboxes component.

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
