using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Models
{
    public class BreadcrumbViewModel
    {
        public string Error { get; set; } = string.Empty;
        public IPublishedContent? CurrentPage { get; set; }
        public List<BreadcrumbLink> Links { get; set; } = new List<BreadcrumbLink>();
    }

    public struct BreadcrumbLink
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
