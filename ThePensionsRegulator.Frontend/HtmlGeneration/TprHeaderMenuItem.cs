using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprHeaderMenuItem
    {
        public AttributeDictionary? Attributes { get; set; }
        public required string LinkText { get; set; }
        public required string LinkUrl { get; set; }
        public IList<TprHeaderMenuChildItem>? HeaderMenuChildItems { get; set; } = [];

        public TprHeaderMenuItem() { }

        [SetsRequiredMembers]
        public TprHeaderMenuItem( string linkText, string linkUrl, IList<TprHeaderMenuChildItem>? subMenuItems = null)
        {
            LinkText = linkText;
            LinkUrl = linkUrl;
            HeaderMenuChildItems = subMenuItems;
        }
    }
}