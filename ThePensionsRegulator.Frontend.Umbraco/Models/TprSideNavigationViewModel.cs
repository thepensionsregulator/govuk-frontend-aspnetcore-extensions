using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.Umbraco.Models
{
    public class TprSideNavigationViewModel
    {
        public TprSideNavigationLink TitleLink { get; set; }
        public List<TprSideNavigationLink> NavigationLinks { get; set; }
    }

    public class TprSideNavigationLink
    {
        public string Name { get; set; }
        public string Url { get; set; }
        bool IsCurrentPage { get; set; }
        List<TprSideNavigationLink> Children { get; set; }
    }
}
