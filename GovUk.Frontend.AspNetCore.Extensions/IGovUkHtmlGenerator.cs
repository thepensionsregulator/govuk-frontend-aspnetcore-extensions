﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Extensions
{
    internal interface IGovUkHtmlGenerator
    {
        TagBuilder GenerateBackToTopLink(string href, IHtmlContent content, AttributeDictionary? attributes);
        TagBuilder GenerateBackToMenu(string href, IHtmlContent content, AttributeDictionary? attributes);
        TagBuilder GenerateTprHeaderBar(string? logoHref, string logoAlt, string? label, IHtmlContent? content, AttributeDictionary? attributes);
        TagBuilder GenerateTprContextBar(string? context1, string? context2, string? context3, AttributeDictionary? attributes);
    }
}
