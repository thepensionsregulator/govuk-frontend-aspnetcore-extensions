using System.IO;
using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprAddressLookupContext
    {
        private (bool IsPageHeading, AttributeDictionary Attributes, IHtmlContent Content)? _legend;

        public AttributeDictionary? LegendAttributes => _legend?.Attributes;
        public IHtmlContent? Legend => _legend?.Content;
        public bool IsLegendPageHeading => _legend?.IsPageHeading ?? false;

        public void SetLegend(bool isPageHeading, AttributeDictionary attributes, IHtmlContent content)
        {
            if (Legend is not null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprAddressLookupLegendTagHelper.TagName,
                    TprAddressLookupTagHelper.TagName);
            }

            _legend = (isPageHeading, attributes, content);
        }

        public void ThrowIfNotComplete()
        {
            if (Legend is null || string.IsNullOrEmpty(GetHtmlString(Legend)))
            {
                throw ExceptionHelper.AChildElementMustBeProvided(TprAddressLookupLegendTagHelper.TagName);
            }
        }

        private static string GetHtmlString(IHtmlContent content)
        {
            using var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
