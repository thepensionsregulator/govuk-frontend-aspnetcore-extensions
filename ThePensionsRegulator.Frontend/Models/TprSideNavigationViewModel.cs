using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.Models
{
    public class TprSideNavigationViewModel
    {
        public required TprSideNavigationLink TitleLink { get; set; }
        public string AriaNavigationLabel { get; set; } = "Pages in this section";
        public List<TprSideNavigationLink> NavigationLinks { get; set; } = new();
    }
}
