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

            var originalText = "This is the original text.";
            var overriddenText = $"This text is overridden at {DateTime.Now.ToLongTimeString()}. This time should update when the page is refreshed to confirm caching works correctly.";

            // Filter out a block in the block list and block grid
            viewModel.Page.BlockList!.Filter = block => block.Settings?.Value<string>(_publishedValueFallback, nameof(GovukGrid.CssClassesForRow)) != "filter-this";
            viewModel.Page.Grid!.Filter = block => block.Settings?.Value<string>(_publishedValueFallback, nameof(GovukGrid.CssClassesForRow)) != "filter-this";

            // Override content in the block list and block grid
            var blockListContent = viewModel.Page.BlockList.First(x => x.GridRowClassList().Contains("override-this"))?.Content;
            blockListContent?.OverrideValue(nameof(GovukTypography.Text), blockListContent.Value<string>(_publishedValueFallback, nameof(GovukTypography.Text))?.Replace(originalText, overriddenText) ?? "");

            var blockGridContent = viewModel.Page.Grid.First(x => x.GridRowClassList().Contains("override-this"))?.Content;
            blockGridContent?.OverrideValue(nameof(GovukTypography.Text), blockGridContent.Value<string>(_publishedValueFallback, nameof(GovukTypography.Text))?.Replace(originalText, overriddenText) ?? "");

            // Override content in a nested block list
            var row = viewModel.Page.BlockList.First(x => x.Content.ContentType.Alias == GovukGridRow.ModelTypeAlias);
            var col = row.Content.Value<OverridableBlockListModel>(_publishedValueFallback, nameof(GovukGridRow.Blocks))?.LastOrDefault(x => x.Content.ContentType.Alias == GovukGridColumn.ModelTypeAlias);
            if (col != null)
            {
                var nestedContent = col.Content.Value<OverridableBlockListModel>(_publishedValueFallback, nameof(GovukGridColumn.Blocks))?.FirstOrDefault(x => x.GridRowClassList().Contains("override-this"))?.Content;
                nestedContent?.OverrideValue("text", nestedContent.Value<string>(_publishedValueFallback, "text")?.Replace(originalText, overriddenText) ?? "");
            }

            return CurrentTemplate(viewModel);
        }
    }
}
