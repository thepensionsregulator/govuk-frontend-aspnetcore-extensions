# Select

For examples see [ASP.NET syntax for the Select component](https://github.com/x-govuk/govuk-frontend-aspnetcore/blob/main/docs/components/select.md).

## Umbraco

You can add a select component to a block grid or block list in Umbraco. For examples of this component in use, see the 'Select' page in the Umbraco example app.

See [Validation](/docs/umbraco/validation.md) for how to validate a select component.

You can configure a fixed set of options in the Umbraco backoffice, or you can supply options at runtime from a database or other data source.

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
        IOverridablePublishedElementFactory _publishedElementFactory,
        IOverridableBlockModelFilterStoreAccessor _filterStoreAccessor)
        ) : RenderController(_logger, _compositeViewEngine, _umbracoContextAccessor)
{
    [ModelType(typeof(ExampleViewModel))]
    public override IActionResult Index()
    {
        var viewModel = new ExampleViewModel
        {
            Page = new ExampleModelsBuilderModel(CurrentPage, _publishedValueFallback)
        };

        var block = viewModel.Page.Blocks.FindBlockByContentTypeAlias(GovukSelect.ModelTypeAlias);
        block.Content.OverrideSelectOptions(new[] { new SelectOption("1", "Hello world") }, _publishedContentTypeCache, _variationContextAccessor, _publishedValueFallback, _publishedElementFactory, _filterStoreAccessor);

        return CurrentTemplate(viewModel);
    }
}
```
