using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprMobileMenuChildItemsContext
    {
        private (AttributeDictionary Attributes, string? LinkText, string? LinkDestination)? _menuChildItem;
        public AttributeDictionary? Attributes { get; set; }
        public string? LinkText => _menuChildItem?.LinkText;
        public string? LinkDestination => _menuChildItem?.LinkDestination;

        public void SetChildItem(AttributeDictionary attributes, string? linkText, string? linkDestination)
        {
            _menuChildItem = (attributes, linkText, linkDestination);
        }
    }
}
