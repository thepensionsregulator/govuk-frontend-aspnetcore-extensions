using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ThePensionsRegulator.GovUk.Frontend;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.GovUk.Frontend.Validation;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class TaskListController : RenderController
    {
        private readonly IPublishedValueFallback _publishedValueFallback;
        public TaskListController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor, IPublishedValueFallback publishedValueFallback) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedValueFallback = publishedValueFallback;
        }

        [ModelType(typeof(TaskList))]
        public override IActionResult Index()
        {
            var viewModel = new TaskList(CurrentPage, _publishedValueFallback);

            // Override content in the block list
            var target = viewModel.Blocks!.FindBlockByClass("yet-another-thing");
            if (target != null)
            {
                target.Settings?.OverrideValue(nameof(GovukTaskSettings.Status), TaskListTaskStatus.Completed.ToString());
                target.Settings?.OverrideValue(nameof(GovukTaskSettings.StatusText), "Done");
            }

            return CurrentTemplate(viewModel);
        }
    }
}
