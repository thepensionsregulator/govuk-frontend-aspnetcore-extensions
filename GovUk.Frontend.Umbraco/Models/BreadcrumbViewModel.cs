using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.Models
{
    public class BreadcrumbViewModel
    {
        public string Error { get; set; } = string.Empty;
        public IPublishedContent? CurrentPage { get; set; }
        public List<BreadcrumbLink> Ancestors { get; set; } = new List<BreadcrumbLink>();
    }

    public struct BreadcrumbLink
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
