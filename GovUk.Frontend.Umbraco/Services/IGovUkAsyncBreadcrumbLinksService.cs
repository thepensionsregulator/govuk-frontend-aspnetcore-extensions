using GovUk.Frontend.Umbraco.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.Services
{
    public interface IGovUkAsyncBreadcrumbLinksService
    {
        public Task<BreadcrumbViewModel> GetLinks(IPublishedContent page);
    }
}
