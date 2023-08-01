using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.BlockLists;
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

		[ModelType(typeof(SummaryCard))]
		public override IActionResult Index()
		{
			var viewModel = new SummaryCard(CurrentPage, null);

			// Override content in the block list
			var target = viewModel.Blocks!.FindBlockByClass("full-name");
			if (target != null)
			{
				target.Content.OverrideValue(nameof(GovukSummaryListItem.ItemValue), "Sarah Smith");
			}

			return CurrentTemplate(viewModel);
		}
	}
}
