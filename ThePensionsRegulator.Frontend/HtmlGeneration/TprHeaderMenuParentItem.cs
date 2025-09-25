using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprHeaderMenuParentItem
    {
        public AttributeDictionary? Attributes { get; set; }
        public Guid ContentKey { get; set; }
        public required string LinkText { get; set; }
        public required string LinkUrl { get; set; }
        public List<TprHeaderMenuChildItem>? HeaderMenuChildItems { get; set; } = new();

        public TprHeaderMenuParentItem() { }

        [SetsRequiredMembers]
        public TprHeaderMenuParentItem(Guid contentKey, string linkText, string linkUrl, List<TprHeaderMenuChildItem>? subMenuItems = null)
        {
            ContentKey = contentKey;
            LinkText = linkText;
            LinkUrl = linkUrl;
            HeaderMenuChildItems = subMenuItems;
        }
    }
}