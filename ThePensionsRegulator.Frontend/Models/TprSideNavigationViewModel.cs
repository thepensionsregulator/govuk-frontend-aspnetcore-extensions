using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.Models
{
    public class TprSideNavigationViewModel
    {
        public required TprSideNavigationLink TitleLink { get; set; }
        public string AriaNavigationLabel { get; set; } = "Pages in this section";
        public string ExpandItemLabel { get; set; } = "Expand {0}";
        public string CollapseItemLabel { get; set; } = "Collapse {0}";
        public string ExpandedItemLabel { get; set; } = "{0} is expanded";
        public string CollapsedItemLabel { get; set; } = "{0} is collapsed";
        public List<TprSideNavigationLink> NavigationLinks { get; set; } = new();
    }
}
