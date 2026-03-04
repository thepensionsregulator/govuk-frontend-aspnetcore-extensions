using HtmlAgilityPack;
using ThePensionsRegulator.GovUk.Frontend.Typography;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.PropertyEditors.ValueFormatters
{
    public abstract class RichTextPropertyValueFormatterBase
    {
        protected IHtmlEncodedString ApplyGovUkTypographyToRichText(object value, TypographyOptions? options = null)
        {
            var govukHtml = GovUkTypography.Apply(
                value is IHtmlEncodedString html ? html.ToHtmlString() : value as string,
                options
            );

            if (!string.IsNullOrWhiteSpace(govukHtml))
            {
                var document = new HtmlDocument();
                document.LoadHtml(govukHtml);

                document = ApplyPermittedStylesToParagraphs(document);
                document = ApplyPermittedStylesToUnorderedLists(document);
                document = ApplyPermittedStylesToOrderedLists(document);
                document = RemoveRemainingStyleAttributes(document);

                govukHtml = document.DocumentNode.OuterHtml;
            }

            return new HtmlEncodedString(govukHtml);
        }

        /// <summary>
        /// TipTap (and TinyMCE before it) automatically surrounds text in a paragraph. Remove that paragraph unless it has a class applied.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        protected static IHtmlEncodedString RemoveWrappingParagraphIfNoClass(IHtmlEncodedString html)
        {
            var richTextHtml = html.ToHtmlString();
            if (!string.IsNullOrWhiteSpace(richTextHtml))
            {
                var document = new HtmlDocument();
                document.LoadHtml(richTextHtml);

                if (document.DocumentNode.ChildNodes.Count == 1 &&
                document.DocumentNode.FirstChild.NodeType == HtmlNodeType.Element &&
                document.DocumentNode.FirstChild.Name == "p" &&
                (string.IsNullOrWhiteSpace(document.DocumentNode.FirstChild.GetAttributeValue("class", null!)) ||
                 document.DocumentNode.FirstChild.GetAttributeValue("class", null!) == "govuk-body"))
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
        /// TinyMCE's unordered lists button has a setting for selecting the style of the unordered list, so we need to enable it or it looks broken.
        /// However it is serialized to the style attribute which we don't want to allow free use of, so convert permitted style attribute values
        /// to classes for display, and remove any others.
        /// </summary>
        /// <param name="document"></param>
        private static HtmlDocument ApplyPermittedStylesToUnorderedLists(HtmlDocument document)
        {
            var permittedStyleAttributes = new Dictionary<string, string> {
                {"list-style-type: circle;" , "govuk-list--circle" },
                {"list-style-type: square;" , "govuk-list--square" }
            };

            return ApplyPermittedStylesToElements(document, permittedStyleAttributes, "ul", "govuk-list--bullet");
        }

        /// <summary>
        /// TinyMCE's ordered lists button has a setting for selecting the style of the ordered list, so we need to enable it or it looks broken.
        /// However it is serialized to the style attribute which we don't want to allow free use of, so convert permitted style attribute values
        /// to classes for display, and remove any others.
        /// </summary>
        /// <param name="document"></param>
        private static HtmlDocument ApplyPermittedStylesToOrderedLists(HtmlDocument document)
        {
            var permittedStyleAttributes = new Dictionary<string, string> {
                {"list-style-type: lower-alpha;" , "govuk-list--lower-alpha" },
                {"list-style-type: lower-greek;" , "govuk-list--lower-greek" },
                {"list-style-type: lower-roman;" , "govuk-list--lower-roman" },
                {"list-style-type: upper-alpha;" , "govuk-list--upper-alpha" },
                {"list-style-type: upper-roman;" , "govuk-list--upper-roman" }
            };

            return ApplyPermittedStylesToElements(document, permittedStyleAttributes, "ol", "govuk-list--number");
        }

        /// <summary>
        /// TinyMCE's alignment buttons are serialized to the style attribute which we don't want to allow free use of, 
        /// so convert permitted style attribute values to classes for display, and remove any others.
        /// </summary>
        /// <param name="document"></param>
        private static HtmlDocument ApplyPermittedStylesToParagraphs(HtmlDocument document)
        {
            var permittedStyleAttributes = new Dictionary<string, string> {
                {"text-align: center;" , "govuk-!-text-align-centre" },
                {"text-align: right;" , "govuk-!-text-align-right" },
                
                // Indents last saved using TinyMCE
                {"padding-left: 40px", "govuk-!-padding-left-7" },
                {"padding-left: 80px", "govuk-!-padding-left-14" },
                {"padding-left: 120px", "govuk-!-padding-left-21" },
                {"padding-left: 160px", "govuk-!-padding-left-28" },
                {"padding-left: 200px", "govuk-!-padding-left-35" },

                // Indents last saved using TipTap
                {"text-indent: 1rem", "govuk-!-padding-left-7" },
                {"text-indent: 2rem", "govuk-!-padding-left-14" },
                {"text-indent: 3rem", "govuk-!-padding-left-21" },
                {"text-indent: 4rem", "govuk-!-padding-left-28" },
                {"text-indent: 5rem", "govuk-!-padding-left-35" }

            };

            return ApplyPermittedStylesToElements(document, permittedStyleAttributes, "p", null);
        }

        private static HtmlDocument ApplyPermittedStylesToElements(HtmlDocument document, Dictionary<string, string> permittedStyleAttributes, string tagName, string? defaultStyleClass)
        {
            var nodes = document.DocumentNode.SelectNodes($"//{tagName}[@style]");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    foreach (var permittedStyle in permittedStyleAttributes.Keys)
                    {
                        if (node.Attributes["style"].Value.Contains(permittedStyle))
                        {
                            if (defaultStyleClass is not null) { node.RemoveClass(defaultStyleClass); }
                            node.AddClass(permittedStyleAttributes[permittedStyle]);
                            break;
                        }
                    }

                    node.Attributes.Remove("style");
                }
            }

            return document;
        }

        private static HtmlDocument RemoveRemainingStyleAttributes(HtmlDocument document)
        {
            var nodes = document.DocumentNode.SelectNodes("//*[@style]");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    node.Attributes.Remove("style");
                }
            }
            return document;
        }
    }
}