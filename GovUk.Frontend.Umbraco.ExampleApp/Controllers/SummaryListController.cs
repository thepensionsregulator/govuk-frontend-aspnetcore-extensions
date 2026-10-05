using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Models;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Services;
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

            OverrideSummaryListItems(viewModel);

            return CurrentTemplate(viewModel);
        }

        private void OverrideSummaryListItems(SummaryList viewModel)
        {
            var summaryListToOverride = viewModel.Blocks!.FindBlockByClass("new-enabled");
            if (summaryListToOverride is null) return;

            var items = CreateTestItems();

            summaryListToOverride.Content.OverrideSummaryListItems(items, _publishedContentTypeCache, _variationContextAccessor);

        }

        private List<SummaryListItem> CreateTestItems()
        {
            var items = new List<SummaryListItem>();

            for (var i = 1; i <= 3; i++)
            {
                var summaryListItem = new SummaryListItem($"Data source item {i}", new HtmlEncodedString($"Data source item value {i}")) { TrackingId = $"tracking-id-{i}" };
                summaryListItem.Actions.Add(new SummaryListAction(new Link { Url = "https://www.example.org" }, $"Action {i}"));
                items.Add(summaryListItem);
            }
            if(!int.TryParse(Request.Query["items"], out var itemCount))
            {
               return items;
            }

            itemCount = Math.Clamp(itemCount, 0, 10);

            for(var i = 1; i <= itemCount; i++)
            {
                var id = i + 3;
                items.Add(new SummaryListItem($"Data source item {id}", new HtmlEncodedString($"Data source item value {id}")) { TrackingId = $"tracking-id-{id}" });
            }

            return items;
        }
    }
}
