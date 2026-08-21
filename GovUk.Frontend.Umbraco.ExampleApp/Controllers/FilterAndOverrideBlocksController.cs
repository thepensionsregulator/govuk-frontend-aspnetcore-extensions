using GovUk.Frontend.Umbraco.ExampleApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.GovUk.Frontend.Validation;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class FilterAndOverrideBlocksController : RenderController
    {
        private readonly IPublishedValueFallback _publishedValueFallback;

        public FilterAndOverrideBlocksController(ILogger<RenderController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            IPublishedValueFallback publishedValueFallback) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedValueFallback = publishedValueFallback;
        }

        [ModelType(typeof(FilterAndOverrideBlocksViewModel))]
        public override IActionResult Index()
        {
            var viewModel = new FilterAndOverrideBlocksViewModel
            {
                Page = new FilterAndOverrideBlocks(CurrentPage, _publishedValueFallback)
            };

            // Filter out a block in the block list and block grid
            viewModel.Page.BlockList!.Filter = block => block.Settings?.Value<string>(_publishedValueFallback, nameof(GovukGrid.CssClassesForRow)) != "filter-this";
            viewModel.Page.Grid!.Filter = block => block.Settings?.Value<string>(_publishedValueFallback, nameof(GovukGrid.CssClassesForRow)) != "filter-this";

            // Override content in the block list and block grid
            viewModel.Page.BlockList.First(x => x.GridRowClassList().Contains("override-this"))?
                .Content.OverrideValue(nameof(GovukTypography.Text), "<p><strong>This text is overridden.</strong></p>");

            viewModel.Page.Grid.First(x => x.GridRowClassList().Contains("override-this"))?
                .Content.OverrideValue(nameof(GovukTypography.Text), "<p><strong>This text is overridden.</strong></p>");

            // Override content in a nested block list
            var row = viewModel.Page.BlockList.First(x => x.Content.ContentType.Alias == GovukGridRow.ModelTypeAlias);
            var col = row.Content.Value<OverridableBlockListModel>(_publishedValueFallback, nameof(GovukGridRow.Blocks))?.LastOrDefault(x => x.Content.ContentType.Alias == GovukGridColumn.ModelTypeAlias);
            if (col != null)
            {
                col.Content.Value<OverridableBlockListModel>(_publishedValueFallback, nameof(GovukGridColumn.Blocks))?.FirstOrDefault(x => x.GridRowClassList().Contains("override-this"))?
                    .Content.OverrideValue("text", "<p><strong>This text is overridden.</strong></p>");
            }

            return CurrentTemplate(viewModel);
        }
    }
}
