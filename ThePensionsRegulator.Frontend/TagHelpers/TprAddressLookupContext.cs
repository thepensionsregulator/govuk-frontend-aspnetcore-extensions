using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Text.Encodings.Web;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprAddressLookupContext
    {
        private (AttributeDictionary Attributes, IHtmlContent Content)? _legend;
        private (AttributeDictionary Attributes, IHtmlContent Content)? _hint;

        public AttributeDictionary? LegendAttributes => _legend?.Attributes;
        public IHtmlContent? Legend => _legend?.Content;

        public AttributeDictionary? HintAttributes => _hint?.Attributes;
        public IHtmlContent? Hint => _hint?.Content;

        public void SetLegend(AttributeDictionary attributes, IHtmlContent content)
        {
            if (Legend is not null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprAddressLookupLegendTagHelper.TagName,
                    TprAddressLookupTagHelper.TagName);
            }

            _legend = (attributes, content);
        }

        public void SetHint(AttributeDictionary attributes, IHtmlContent content)
        {
            if (Hint is not null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprAddressLookupHintTagHelper.TagName,
                    TprAddressLookupTagHelper.TagName);
            }

            _hint = (attributes, content);
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
