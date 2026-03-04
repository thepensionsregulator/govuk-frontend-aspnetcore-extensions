using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using ThePensionsRegulator.GovUk.Frontend;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprRelatedLinksContext
    {
        private (AttributeDictionary Attributes, IHtmlContent Content)? _heading;

        public AttributeDictionary HeadingAttributes => _heading?.Attributes ?? [];
        public IHtmlContent HeadingContent => _heading?.Content ?? new HtmlString(string.Empty);
        public IList<(AttributeDictionary Attributes, IHtmlContent Content)> Links = new List<(AttributeDictionary Attributes, IHtmlContent Content)>();

        public void AddLink(AttributeDictionary attributes, IHtmlContent htmlContent)
        {
            Links.Add((attributes, htmlContent));
        }

        public void SetHeading(AttributeDictionary attributes, IHtmlContent htmlContent)
        {
            if (_heading != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprRelatedLinksHeadingTagHelper.TagName,
                    TprRelatedLinksTagHelper.TagName);
            }

            _heading = (attributes, htmlContent);
        }
    }
}