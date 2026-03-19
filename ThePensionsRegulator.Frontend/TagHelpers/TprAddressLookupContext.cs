using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprAddressLookupContext
    {
        private (AttributeDictionary Attributes, IHtmlContent Content)? _legend;

        public AttributeDictionary? LegendAttributes => _legend?.Attributes;
        public IHtmlContent? Legend => _legend?.Content;

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

        public void ThrowIfNotComplete()
        {
            if (Legend is null)
            {
                throw ExceptionHelper.AChildElementMustBeProvided(TprAddressLookupLegendTagHelper.TagName);
            }
        }
    }
}
