using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.Blocks;
using GovUk.Frontend.Umbraco.ExampleApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System.Linq;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class BlockListController : RenderController
    {
        public BlockListController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }

        [ModelType(typeof(BlockListViewModel))]
        public override IActionResult Index()
        {
            var viewModel = new BlockListViewModel
            {
                Page = new BlockList(CurrentPage, null)
            };

            // Filter out a block in the block list
            viewModel.Page.Blocks!.Filter = block => block.Settings.Value<string>("cssClassesForRow") != "filter-this";

            // Override content in the block list
            viewModel.Page.Blocks.First(x => x.Settings.GridRowClassList().Contains("override-this"))?
                .Content.OverrideValue("text", "<p><strong>This text is overridden.</strong></p>");

            // Override content in a nested block list
            var row = viewModel.Page.Blocks.First(x => x.Content.ContentType.Alias == "govukGridRow");
            var col = row.Content.Value<OverridableBlockListModel>("blocks")?.LastOrDefault(x => x.Content.ContentType.Alias == "govukGridColumn");
            if (col != null)
            {
                col.Content.Value<OverridableBlockListModel>("blocks")?.FirstOrDefault(x => x.Settings.GridRowClassList().Contains("override-this"))?
                    .Content.OverrideValue("text", "<p><strong>This text is overridden.</strong></p>");
            }

            return CurrentTemplate(viewModel);
        }
    }
}
