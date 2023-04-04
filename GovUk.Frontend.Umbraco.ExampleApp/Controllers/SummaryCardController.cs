using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.ExampleApp.Models;
using GovUk.Frontend.Umbraco.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class SummaryCardController : RenderController
    {
        public SummaryCardController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }

        [ModelType(typeof(SummaryCardViewModel))]
        public override IActionResult Index()
        {
            var viewModel = new SummaryCardViewModel
            {
                Page = new SummaryCard(CurrentPage, null)
            };

            // Override content in the block list
            viewModel.Blocks = new OverridableBlockListModel(viewModel.Page.Blocks!);
            var target = viewModel.Blocks.FindBlock(x => x.Content.ContentType.Alias == GovukSummaryListItem.ModelTypeAlias &&
                                                         x.Settings.Value<string>(nameof(GovukSummaryListItemSettings.CssClasses))!.Contains("full-name")) as OverridableBlockListItem;
            if (target != null)
            {
                target.Content.OverrideValue(nameof(GovukSummaryListItem.ItemValue), "Sarah Smith");
            }

            return CurrentTemplate(viewModel);
        }
    }
}
