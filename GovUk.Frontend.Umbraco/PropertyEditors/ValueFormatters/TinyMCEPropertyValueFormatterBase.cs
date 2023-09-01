using GovUk.Frontend.AspNetCore.Extensions.Typography;
using HtmlAgilityPack;
using System.Collections.Generic;
using Umbraco.Cms.Core.Strings;

namespace GovUk.Frontend.Umbraco.PropertyEditors.ValueFormatters
{
    public abstract class TinyMCEPropertyValueFormatterBase
    {
        protected IHtmlEncodedString ApplyGovUkTypographyToTinyMCE(object value, TypographyOptions? options = null)
        {
            return ApplyPermittedStylesToOrderedLists(
                new HtmlEncodedString(
                    GovUkTypography.Apply(
                        value is IHtmlEncodedString html ? html.ToHtmlString() : value as string,
                        options
                    )
                )
            );
        }

        /// <summary>
        /// TinyMCE automatically surrounds text in a paragraph. Remove that paragraph.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        protected static IHtmlEncodedString RemoveWrappingParagraph(IHtmlEncodedString html)
        {
            if (!string.IsNullOrWhiteSpace(html.ToHtmlString()))
            {
                var document = new HtmlDocument();
                document.LoadHtml(html.ToHtmlString());

                if (document.DocumentNode.ChildNodes.Count == 1 &&
                document.DocumentNode.FirstChild.NodeType == HtmlNodeType.Element &&
                document.DocumentNode.FirstChild.Name == "p")
                {
                    html = new HtmlEncodedString(document.DocumentNode.FirstChild.InnerHtml);
                }
                else
                {
                    html = new HtmlEncodedString(document.DocumentNode.OuterHtml);
                }
            }
            return html;
        }


        /// <summary>
        /// TinyMCE's ordered lists button has a setting for selecting the style of the ordered list, so we need to enable it or it looks broken.
        /// However it is serialized to the style attribute which we don't want to allow free use of, so convert permitted style attribute values
        /// to classes for display, and remove any others.
        /// </summary>
        /// <param name="document"></param>
        protected static IHtmlEncodedString ApplyPermittedStylesToOrderedLists(IHtmlEncodedString html)
        {
            var permittedStyleAttributes = new Dictionary<string, string> {
                {"list-style-type: lower-alpha;" , "govuk-list--lower-alpha" },
                {"list-style-type: lower-greek;" , "govuk-list--lower-greek" },
                {"list-style-type: lower-roman;" , "govuk-list--lower-roman" },
                {"list-style-type: upper-alpha;" , "govuk-list--upper-alpha" },
                {"list-style-type: upper-roman;" , "govuk-list--upper-roman" }
            };

            if (!string.IsNullOrWhiteSpace(html.ToHtmlString()))
            {
                var document = new HtmlDocument();
                document.LoadHtml(html.ToHtmlString());

                var nodes = document.DocumentNode.SelectNodes("//ol[@style]");
                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        foreach (var permittedStyle in permittedStyleAttributes.Keys)
                        {
                            if (node.Attributes["style"].Value.Contains(permittedStyle))
                            {
                                node.RemoveClass("govuk-list--number");
                                node.AddClass(permittedStyleAttributes[permittedStyle]);
                                break;
                            }
                        }

                        node.Attributes.Remove("style");
                    }

                    html = new HtmlEncodedString(document.DocumentNode.OuterHtml);
                }
            }

            return html;
        }
    }
}