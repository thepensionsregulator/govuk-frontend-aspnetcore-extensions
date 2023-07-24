using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using ThePensionsRegulator.Umbraco.BlockLists;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class TaskListController : RenderController
    {
        public TaskListController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }

        [ModelType(typeof(TaskList))]
        public override IActionResult Index()
        {
            var viewModel = new TaskList(CurrentPage, null);

            // Override content in the block list
            var target = viewModel.Blocks!.FindBlock(x => x.Content.ContentType.Alias == GovukTask.ModelTypeAlias &&
                                                         x.Settings.Value<string>(nameof(GovukTaskSettings.CssClasses))!.Contains("yet-another-thing"));
            if (target != null)
            {
                target.Settings.OverrideValue(nameof(GovukTaskSettings.Status), TaskListTaskStatus.Completed.ToString());
                target.Settings.OverrideValue(nameof(GovukTaskSettings.StatusText), "Done");
            }

            return CurrentTemplate(viewModel);
        }
    }
}
