using GovUk.Frontend.Umbraco.ExampleApp.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Validation;
using ThePensionsRegulator.GovUk.Frontend.Validation;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class TextInputController : RenderController
    {
        private readonly IPublishedValueFallback _publishedValueFallback;
        public TextInputController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor, IPublishedValueFallback publishedValueFallback) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedValueFallback = publishedValueFallback;
        }

        [ModelType(typeof(TextInputViewModel))]
        public override IActionResult Index()
        {
            if (Request.Query.Keys.Contains("culture"))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(Request.Query["culture"]!)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

                LocalRedirect(Request.Path);
            }

            var viewModel = new TextInputViewModel
            {
                Page = new TextInput(CurrentPage, _publishedValueFallback)
            };

            ModelState.SetInitialValue(nameof(TextInputViewModel.Field7), "Hidden field value");

            return CurrentTemplate(viewModel);
        }
    }
}
