using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.GovUk.Frontend.Validation;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class SummaryCardController : RenderController
    {
        private readonly IPublishedValueFallback _publishedValueFallback;
        public SummaryCardController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor, IPublishedValueFallback publishedValueFallback) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedValueFallback = publishedValueFallback;
        }

        [ModelType(typeof(SummaryCard))]
        public override IActionResult Index()
        {
            var viewModel = new SummaryCard(CurrentPage, _publishedValueFallback);

            // Override content in the block list
            viewModel.Blocks!.FindBlockByClass("full-name")?.Content.OverrideValue(nameof(GovukSummaryListItem.ItemValue), "Sarah Smith");

            return CurrentTemplate(viewModel);
        }
    }
}
