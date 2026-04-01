using System;
using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.ExampleApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class AddressLookupSurfaceController : SurfaceController
    {
        public AddressLookupSurfaceController(IUmbracoDatabaseFactory umbracoDatabaseFactory,
            IUmbracoContextAccessor umbracoContextAccessor,
            ServiceContext context,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider
            )
            : base(umbracoContextAccessor, umbracoDatabaseFactory, context, appCaches, profilingLogger, publishedUrlProvider)
        {
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateUmbracoFormRouteString]
        [ModelType(typeof(AddressLookupViewModel))]
        public IActionResult Index(AddressLookupViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel?.Page?.NextPage != null)
                {
                    Response.StatusCode = 303;
                    Response.GetTypedHeaders().Location = new Uri(viewModel.Page.NextPage.Url(), UriKind.RelativeOrAbsolute);
                }
            }

            ModelState.SetModelValue(nameof(viewModel.ShippingAddressLine1), new(viewModel?.ShippingAddressLine1?.ToString()));
            ModelState.SetModelValue(nameof(viewModel.ShippingAddressLine2), new(viewModel?.ShippingAddressLine2?.ToString()));
            ModelState.SetModelValue(nameof(viewModel.ShippingSomethingReallyRandom), new(viewModel?.ShippingSomethingReallyRandom?.ToString()));
            ModelState.SetModelValue(nameof(viewModel.ShippingCounty), new(viewModel?.ShippingCounty?.ToString()));
            ModelState.SetModelValue(nameof(viewModel.ShippingCountry), new(viewModel?.ShippingCountry?.ToString()));
            ModelState.SetModelValue(nameof(viewModel.ShippingPostcode), new(viewModel?.ShippingPostcode?.ToString()));
            ModelState.SetModelValue(nameof(viewModel.BillingAddressLine1), new(viewModel?.BillingAddressLine1?.ToString()));
            ModelState.SetModelValue(nameof(viewModel.BillingAddressLine2), new(viewModel?.BillingAddressLine2?.ToString()));
            ModelState.SetModelValue(nameof(viewModel.BillingSomethingReallyRandom), new(viewModel?.BillingSomethingReallyRandom?.ToString()));
            ModelState.SetModelValue(nameof(viewModel.BillingCounty), new(viewModel?.BillingCounty?.ToString()));
            ModelState.SetModelValue(nameof(viewModel.BillingCountry), new(viewModel?.BillingCountry?.ToString()));
            ModelState.SetModelValue(nameof(viewModel.BillingPostcode), new(viewModel?.BillingPostcode?.ToString()));

            return View("AddressLookup", viewModel);
        }
    }
}
