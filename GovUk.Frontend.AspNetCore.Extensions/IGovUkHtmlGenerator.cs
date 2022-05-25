using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Extensions
{
    internal interface IGovUkHtmlGenerator
    {
        TagBuilder GenerateBackToTopLink(string href, IHtmlContent content, AttributeDictionary? attributes);
    }
}
