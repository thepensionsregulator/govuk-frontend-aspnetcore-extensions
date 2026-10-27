# Summary list

For examples see [ASP.NET syntax for the Summary list component](https://github.com/x-govuk/govuk-frontend-aspnetcore/blob/main/docs/components/summary-list.md).

## Umbraco

You can add a summary list component to a block grid or block list in Umbraco. For examples of this component in use, see the 'Summary list' page in the Umbraco example app.

You can configure a fixed set of summary list items in the Umbraco backoffice, or you can supply summary list items at runtime from a database or other data source.

```csharp
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Models;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Web.Common.PublishedModels;

public class ExampleController(ILogger<RenderController> _logger,
        ICompositeViewEngine _compositeViewEngine,
        IUmbracoContextAccessor _umbracoContextAccessor,
        IPublishedValueFallback _publishedValueFallback,
        IPublishedContentTypeCache _publishedContentTypeCache,
        IVariationContextAccessor _variationContextAccessor,
        IOverridablePublishedElementFactoryAccessor _publishedElementFactory,
        IOverridableBlockModelFilterStoreAccessor _filterStoreAccessor)
        : RenderController(_logger, _compositeViewEngine, _umbracoContextAccessor)
{
    [ModelType(typeof(ExampleViewModel))]
    public override IActionResult Index()
    {
        var viewModel = new ExampleViewModel
        {
            Page = new ExampleModelsBuilderModel(CurrentPage, _publishedValueFallback)
        };

        var listItem = new SummaryListItem("Example key", new HtmlEncodedString("<em>The value</em>"));
        listItem.Actions.Add(new SummaryListAction(new Link { Url = "https://www.example.org/change-the-thing" }, "Change"));

        var block = viewModel.Page.Blocks.FindBlockByContentTypeAlias(GovukSummaryList.ModelTypeAlias);
        block.Content.OverrideSummaryListItems(new[] { listItem }, _publishedContentTypeCache, _variationContextAccessor, _publishedValueFallback, _publishedElementFactory, _filterStoreAccessor);

        return CurrentTemplate(viewModel);
    }
}
```
