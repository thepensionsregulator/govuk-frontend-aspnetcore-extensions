using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.Models
{
    public class TprSideNavigationLink
    {
        public required string Name { get; set; }
        public required string Url { get; set; }
        public bool IsCurrentPage { get; set; }
        public bool IsExpanded { get; set; }
        public TprSideNavigationLink? Parent { get; set; }
        public List<TprSideNavigationLink> Children { get; set; } = new();
    }
}
