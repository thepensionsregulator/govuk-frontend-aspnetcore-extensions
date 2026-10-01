using Microsoft.AspNetCore.Mvc;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Services;
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
        private readonly ISummaryListNewItemTracker _summaryListNewItemTracker;
        public SummaryListSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, ISummaryListNewItemTracker summaryListNewItemTracker) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _summaryListNewItemTracker = summaryListNewItemTracker;
        }

        [HttpPost]
        public async Task<IActionResult> AddTestItem()
        {
            await _summaryListNewItemTracker.MarkAsNew("tracking-id-4");

            return Redirect($"{ CurrentPage!.Url()}?Items=1");
        }
    }
}
