using GovUk.Frontend.Umbraco.ExampleApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ThePensionsRegulator.GovUk.Frontend.Validation;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class DateInputController : RenderController
    {
        private readonly IPublishedValueFallback _publishedValueFallback;
        public DateInputController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor, IPublishedValueFallback publishedValueFallback) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedValueFallback = publishedValueFallback;
        }

        [ModelType(typeof(DateInputViewModel))]
        public override IActionResult Index()
        {
            var viewModel = new DateInputViewModel
            {
                Page = new DateInput(CurrentPage, _publishedValueFallback)
            };
            return CurrentTemplate(viewModel);
        }
    }
}
