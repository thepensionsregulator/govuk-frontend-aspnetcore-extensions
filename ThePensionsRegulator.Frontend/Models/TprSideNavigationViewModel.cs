using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.Models
{
    public class TprSideNavigationViewModel
    {
        public required TprSideNavigationLink TitleLink { get; set; }
        public string AriaNavigationLabel { get; set; } = "Pages in this section";
        public string ExpandItemLabel { get; set; } = "{0} toggle button";
        public string CollapseItemLabel { get; set; } = "{0} toggle button";
        public string ExpandedItemLabel { get; set; } = "{0} is expanded";
        public string CollapsedItemLabel { get; set; } = "{0} is collapsed";
        public List<TprSideNavigationLink> NavigationLinks { get; set; } = new();
    }
}
