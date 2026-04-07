using ThePensionsRegulator.Frontend.Models;

namespace ThePensionsRegulator.Frontend.Services
{
    /// <summary>
    /// Service to retrieve the side navigation links for the current page.
    /// </summary>
    public interface ITprSideNavigationLinksService
    {
        /// <summary>
        /// Gets a hierarchy of link metadata to display as links in side navigation for the current page.
        /// </summary>
        /// <returns>Metadata for links, or <c>null</c> if there are no links to display.</returns>
        public TprSideNavigationViewModel? GetLinks();
    }
}
