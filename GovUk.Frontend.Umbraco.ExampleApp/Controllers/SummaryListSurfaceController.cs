using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class SummaryListSurfaceController : SurfaceController
    {
        public SummaryListSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        [HttpPost]
        public async Task<IActionResult> AddTestItem()
        {
            var itemCount = 0;
            if (int.TryParse(Request.Query["Items"], out var parsedCount)) { itemCount = parsedCount; }

            itemCount = Math.Clamp(itemCount + 1, 1, 10);

                return Redirect($"{CurrentPage!.Url()}?Items={itemCount}");
        }
    }
}
