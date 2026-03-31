using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;

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
            if (!attributes.ContainsKey("aria-labelledby") && HeadingAttributes.ContainsKey("id"))
            {
                attributes.Add("aria-labelledby", HeadingAttributes["id"]);
            }

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

            if (!attributes.ContainsKey("id"))
            {
                var headingId = new KeyValuePair<string, string?>("id", Guid.NewGuid().ToString());
                attributes.Add(headingId);
                HeadingAttributes.Add(headingId);
            }

            _heading = (attributes, htmlContent);
        }
    }
}