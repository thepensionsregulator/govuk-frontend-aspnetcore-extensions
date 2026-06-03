using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.ExampleApp.Models;
using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class TextInputController : RenderController
    {
        public TextInputController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }

        [ModelType(typeof(TextInputViewModel))]
        public override IActionResult Index()
        {
            if (Request.Query.Keys.Contains("culture"))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(Request.Query["culture"])),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

                LocalRedirect(Request.Path);
            }

            var viewModel = new TextInputViewModel
            {
                Page = new TextInput(CurrentPage, null)
            };

            ModelState.SetInitialValue(nameof(TextInputViewModel.Field7), "Hidden field value");

            return CurrentTemplate(viewModel);
        }
    }
}
