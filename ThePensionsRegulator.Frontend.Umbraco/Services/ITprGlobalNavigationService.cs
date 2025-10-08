using System.Collections.Generic;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public interface ITprGlobalNavigationService
    {
        public IList<TprHeaderMenuItem>? GetMenuItems(IPublishedContent settingsNode, TprHeaderMenuViewModel tprHeaderMenuViewModel);
    }
}
