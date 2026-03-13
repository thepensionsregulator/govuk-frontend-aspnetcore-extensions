using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.ExampleApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class AddressLookupPrimaryAndSecondaryDataProvidedController : RenderController
    {
        public AddressLookupPrimaryAndSecondaryDataProvidedController(ILogger<AddressLookupPrimaryAndSecondaryDataProvidedController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }

        [ModelType(typeof(AddressLookupPrimaryAndSecondaryDataProvidedViewModel))]
        public override IActionResult Index()
        {
            var viewModel = new AddressLookupPrimaryAndSecondaryDataProvidedViewModel
            {
                Page = new AddressLookupPrimaryAndSecondaryDataProvided(CurrentPage, null),
            };

            return CurrentTemplate(viewModel);
        }
    }
}
