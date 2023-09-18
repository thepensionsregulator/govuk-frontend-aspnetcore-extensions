using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.ExampleApp.Models;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Website.Controllers;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class BlockGridSurfaceController : SurfaceController
    {
        public BlockGridSurfaceController(IUmbracoDatabaseFactory umbracoDatabaseFactory,
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
        [ModelType(typeof(BlockGridViewModel))]
        public IActionResult Index(BlockGridViewModel viewModel)
        {
            return View("BlockGrid", viewModel);
        }

    }
}
