using System.Collections.Generic;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public interface ITprGlobalNavigationSerivce
    {
        public List<TprHeaderMenuParentItem> GetMenuItems(int rootId);
        public List<TprHeaderMenuParentItem> AddParentMenuItem(int rootId, int placement, TprHeaderMenuParentItem parentMenuItem);
        public List<TprHeaderMenuParentItem> AddChildMenuItem(int rootId, int placement, int hierarchy, TprHeaderMenuChildItem childMenuItem);

    }
}
