using ThePensionsRegulator.Frontend.Models;

namespace ThePensionsRegulator.Frontend.Services
{
    public interface ITprSideNavigationLinksService
    {
        public TprSideNavigationViewModel? GetLinks();
    }
}
