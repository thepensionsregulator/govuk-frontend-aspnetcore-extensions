using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Models;
using ThePensionsRegulator.GovUk.Frontend.Validation;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class SummaryListController(
        ILogger<RenderController> _logger,
        ICompositeViewEngine _compositeViewEngine,
        IUmbracoContextAccessor _umbracoContextAccessor,
        IPublishedContentTypeCache _publishedContentTypeCache,
        IVariationContextAccessor _variationContextAccessor,
        IPublishedValueFallback _publishedValueFallback,
        IOverridablePublishedElementFactory _publishedElementFactory,
        IOverridableBlockModelFilterStoreAccessor _filterStoreAccessor)
        : RenderController(_logger, _compositeViewEngine, _umbracoContextAccessor)
    {
        [ModelType(typeof(SummaryList))]
        public override IActionResult Index()
        {
            var viewModel = new SummaryList(CurrentPage, _publishedValueFallback);

            // Override content in a summary list
            var summaryListToOverride = viewModel.Blocks!.FindBlockByClass("override-this");
            if (summaryListToOverride is not null)
            {
                var summaryListItems = new List<SummaryListItem>();
                for (var i = 1; i <= 5; i++)
                {
                    var summaryListItem = new SummaryListItem($"Data source item {i}", new HtmlEncodedString($"Data source item value {i}"));
                    summaryListItem.Actions.Add(new SummaryListAction(new Link { Url = "https://www.example.org" }, $"Action {i}"));
                    summaryListItems.Add(summaryListItem);
                }
                summaryListToOverride.Content.OverrideSummaryListItems(summaryListItems, _publishedContentTypeCache, _variationContextAccessor, _publishedValueFallback, _publishedElementFactory, _filterStoreAccessor);
            }

            return CurrentTemplate(viewModel);
        }
    }
}
