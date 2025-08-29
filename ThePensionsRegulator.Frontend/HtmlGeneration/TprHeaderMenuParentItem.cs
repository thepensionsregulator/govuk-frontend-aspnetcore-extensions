using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprHeaderMenuParentItem
    {
        public AttributeDictionary? Attributes { get; set; }
        public string? LinkText { get; set; }
        public string? LinkDestination { get; set; }
        public List<TprHeaderMenuChildItem>? HeaderMenuChildItems { get; set; } = new();

        public TprHeaderMenuParentItem(){}

        public TprHeaderMenuParentItem(string? linkText, string? linkDestination, List<TprHeaderMenuChildItem>? subMenuItems = null)
        {
            LinkText = linkText;
            LinkDestination = linkDestination;
            HeaderMenuChildItems = subMenuItems;
        }
    }
}