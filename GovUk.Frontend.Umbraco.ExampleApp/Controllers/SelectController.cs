using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.Blocks;
using GovUk.Frontend.Umbraco.ExampleApp.Models;
using GovUk.Frontend.Umbraco.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class SelectController : RenderController
    {
        private readonly IPublishedValueFallback _publishedValueFallback;
        private readonly IPublishedContentTypeCache _publishedContentTypeCache;
        private readonly IVariationContextAccessor _variationContextAccessor;

        public SelectController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor,
            IPublishedValueFallback publishedValueFallback, IPublishedContentTypeCache publishedContentTypeCache, IVariationContextAccessor variationContextAccessor) :
            base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedValueFallback = publishedValueFallback;
            _publishedContentTypeCache = publishedContentTypeCache;
            _variationContextAccessor = variationContextAccessor;
        }

        [ModelType(typeof(SelectViewModel))]
        public override IActionResult Index()
        {
            var viewModel = new SelectViewModel
            {
                Page = new Select(CurrentPage, _publishedValueFallback)
            };

            var optionsFromDataSource = new SelectOption[]
            {
                new SelectOption(string.Empty, string.Empty),
                new SelectOption("1", "Option 1 from controller"),
                new SelectOption("2", "Option 2 from controller"),
                new SelectOption("3", "Option 3 from controller"),
                new SelectOption("4", "Option 4 from controller")
            };

            viewModel.Page.Blocks!.FindBlockByClass("external-data")!
                .Content
                .OverrideSelectOptions(optionsFromDataSource, _publishedContentTypeCache, _variationContextAccessor, viewModel.Page.Blocks!.Filter);

            return CurrentTemplate(viewModel);
        }
    }
}
