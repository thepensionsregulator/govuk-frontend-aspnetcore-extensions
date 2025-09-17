using System;
using System.Collections.Generic;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public interface ITprGlobalNavigationService
    {
        public IList<TprHeaderMenuParentItem> GetMenuItems(Guid rootKey);
        public IList<TprHeaderMenuParentItem> AddParentMenuItem(Guid rootKey, int placement, TprHeaderMenuParentItem parentMenuItem);
        public IList<TprHeaderMenuParentItem> AddChildMenuItem(Guid rootKey, int placement, int hierarchy, TprHeaderMenuChildItem childMenuItem);
    }
}
