using ThePensionsRegulator.Frontend.Umbraco.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public interface IBreadcrumbLinksService
    {
        public BreadcrumbViewModel GetLinks(IPublishedContent Page);
    }
}
