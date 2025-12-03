using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprHeaderMenuChildItemContext
    {
        private (AttributeDictionary Attributes, string LinkText, string LinkUrl) _menuChildItem;
        public AttributeDictionary? Attributes { get; set; }
        public string LinkText => _menuChildItem.LinkText;
        public string LinkUrl => _menuChildItem.LinkUrl;

        public void SetChildItem(AttributeDictionary attributes, string linkText, string linkUrl)
        {
            _menuChildItem = (attributes, linkText, linkUrl);
        }
    }
}
