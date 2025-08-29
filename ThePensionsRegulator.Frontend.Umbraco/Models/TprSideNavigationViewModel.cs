using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.Umbraco.Models
{
    public class TprSideNavigationViewModel
    {
        public required TprSideNavigationLink TitleLink { get; set; }
        public List<TprSideNavigationLink> NavigationLinks { get; set; } = new();
    }
}
