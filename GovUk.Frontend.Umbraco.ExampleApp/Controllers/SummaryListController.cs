using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Models;
using ThePensionsRegulator.GovUk.Frontend.Validation;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class SummaryListController : RenderController
    {
        private readonly IPublishedContentTypeCache _publishedContentTypeCache;
        private readonly IVariationContextAccessor _variationContextAccessor;
        private readonly IPublishedValueFallback _publishedValueFallback;
        public SummaryListController(ILogger<RenderController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            IPublishedContentTypeCache publishedContentTypeCache,
            IVariationContextAccessor variationContextAccessor,
            IPublishedValueFallback publishedValueFallback)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedContentTypeCache = publishedContentTypeCache ?? throw new ArgumentNullException(nameof(publishedContentTypeCache));
            _variationContextAccessor = variationContextAccessor ?? throw new ArgumentNullException(nameof(variationContextAccessor));
            _publishedValueFallback = publishedValueFallback ?? throw new ArgumentNullException(nameof(publishedValueFallback));
        }

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
                summaryListToOverride.Content.OverrideSummaryListItems(summaryListItems, _publishedContentTypeCache, _variationContextAccessor);
            }

            return CurrentTemplate(viewModel);
        }
    }
}
