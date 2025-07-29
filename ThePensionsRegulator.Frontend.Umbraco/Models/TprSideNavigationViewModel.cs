using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Frontend.Umbraco.Models
{
    public class TprSideNavigationViewModel
    {
        public IPublishedContent CurrentPage { get; set; }
        public IPublishedContent RootAncestor { get; set; }
        public string[] BlacklistedUrls { get; set; } = [];
    }
}
