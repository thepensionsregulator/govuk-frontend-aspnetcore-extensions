using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.Umbraco.Models
{
    public class TprSideNavigationViewModel
    {
        public TprSideNavigationLink TitleLink { get; set; }
        public List<TprSideNavigationLink> NavigationLinks { get; set; } = new();
    }

    public class TprSideNavigationLink
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsCurrentPage { get; set; }
        public bool IsExpanded { get; set; }
        public TprSideNavigationLink Parent { get; set; }
        public List<TprSideNavigationLink> Children { get; set; } = new();
    }
}
