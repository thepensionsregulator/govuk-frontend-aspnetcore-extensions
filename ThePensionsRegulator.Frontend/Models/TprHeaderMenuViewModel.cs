using System.Diagnostics.CodeAnalysis;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public class TprHeaderMenuViewModel
    {
        [SetsRequiredMembers]
        public TprHeaderMenuViewModel(string menuAlias, string linkTextAlias, string linkUrlAlias, string menuItemChildAlias)
        {
            MenuBlockListAlias = menuAlias;
            LinkTextAlias = linkTextAlias;
            LinkUrlAlias = linkUrlAlias;
            MenuItemsChildAlias = menuItemChildAlias;
        }
        public required string MenuBlockListAlias { get; set; }
        public required string LinkTextAlias { get; set; }
        public required string LinkUrlAlias { get; set; }
        public required string MenuItemsChildAlias { get; set; }
    }
}
