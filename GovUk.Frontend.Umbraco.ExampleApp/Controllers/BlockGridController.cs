using GovUk.Frontend.AspNetCore.Extensions.Validation;
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
    public class BlockGridController : RenderController
    {
        public BlockGridController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }

        [ModelType(typeof(BlockGridViewModel))]
        public override IActionResult Index()
        {
            var viewModel = new BlockGridViewModel
            {
                Page = new BlockGrid(CurrentPage, null)
            };

            // Filter out a block within the block grid, and in an area within the block grid,
            // and in a block list within an area within the block grid (the third option in the select)
            viewModel.Page.Blocks!.Filter = block => block.Settings?.Value<string>("cssClasses") != "filter-this" && block.Content.Value<string>("label") != "three";

            // Override a value for a block inside an area inside a block grid by searching the grid object
            var textInputBlock = viewModel.Page.Blocks!.FindBlockByContentTypeAlias(GovukTextInput.ModelTypeAlias);
            if (textInputBlock != null)
            {
                textInputBlock.Content.OverrideValue(nameof(GovukTextarea.Hint), "This hint in overridden by accessing the block from the BlockGridModel.");
            }

            // Override a value for a block inside an area inside a block grid by searching the area object
            var firstArea = viewModel.Page.Blocks!.First(x => x.Areas.Any()).Areas.First();
            var textareaBlock = firstArea.FindBlockByContentTypeAlias(GovukTextarea.ModelTypeAlias);
            if (textareaBlock != null)
            {
                textareaBlock.Content.OverrideValue(nameof(GovukTextarea.Hint), "This hint in overridden by accessing the block via the area.");
            }

            // Override a value for a block inside a block list inside an area inside a block grid
            var selectOption = viewModel.Page.Blocks!.FindBlockByContentTypeAlias(GovukSelectOption.ModelTypeAlias);
            if (selectOption != null)
            {
                selectOption.Content.OverrideValue(nameof(GovukSelectOption.Label), "overridden");
            }

            return CurrentTemplate(viewModel);
        }
    }
}
