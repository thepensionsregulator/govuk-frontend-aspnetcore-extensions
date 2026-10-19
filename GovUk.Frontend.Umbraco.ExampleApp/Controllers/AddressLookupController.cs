using GovUk.Frontend.Umbraco.ExampleApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Umbraco.Models;
using ThePensionsRegulator.Frontend.Validation;
using ThePensionsRegulator.GovUk.Frontend.Validation;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class AddressLookupController : RenderController
    {
        private readonly IPublishedValueFallback _publishedValueFallback;

        public AddressLookupController(ILogger<AddressLookupController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor, IPublishedValueFallback publishedValueFallback) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedValueFallback = publishedValueFallback;
        }

        [ModelType(typeof(AddressLookupViewModel))]
        public override IActionResult Index()
        {
            var viewModel = new AddressLookupViewModel
            {
                Page = new AddressLookup(CurrentPage, _publishedValueFallback),
                ShippingAddress = new UmbracoTprAddress
                {
                    UPRNReference = "123",
                    AddressLine1 = "123 Main St",
                    AddressLine2 = "Apt 4B",
                    AddressLine3 = "District 9",
                    PostTown = "London",
                    PostCounty = "Greater London",
                    Postcode = "SW1A 1AA",
                    CountryId = 1
                },
                BillingAddress = new UmbracoTprAddress
                {
                    UPRNReference = "123",
                    AddressLine1 = "123 Main St",
                    AddressLine2 = "Apt 4B",
                    AddressLine3 = "District 9",
                    PostTown = "London",
                    PostCounty = "Greater London",
                    Postcode = "SW1A 1AA",
                    CountryId = 1
                }
            };

            ModelState.SetInitialValues(viewModel.ShippingAddress, nameof(viewModel.ShippingAddress));
            ModelState.SetInitialValues(viewModel.BillingAddress, nameof(viewModel.BillingAddress));

            return CurrentTemplate(viewModel);
        }
    }
}
