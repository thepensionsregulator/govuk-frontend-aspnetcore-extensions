using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace GovUk.Frontend.Umbraco.Typography
{
    public static class GovUkTypography
    {
        public static string Apply(string? html, TypographyOptions? options = null)
        {
            if (!string.IsNullOrWhiteSpace(html))
            {
                options = options ?? new TypographyOptions();

                var document = new HtmlDocument();
                document.LoadHtml(html);

                ApplyClass(document, "//a", "govuk-link");
                if (options.BackgroundType == BackgroundType.Dark)
                {
                    ApplyInverseClasses(document);
                }

                var allHeadingClasses = new[] { "govuk-heading-xl", "govuk-heading-l", "govuk-heading-m", "govuk-heading-s" };
                ApplyClass(document, "//h2", "govuk-heading-m", allHeadingClasses);
                ApplyClass(document, "//h3", "govuk-heading-s", allHeadingClasses);
                ApplyClass(document, "//p", "govuk-body");
                ApplyClass(document, "//ul", "govuk-list");
                ApplyClass(document, "//ul", "govuk-list--bullet");
                ApplyClass(document, "//ol", "govuk-list");
                ApplyClass(document, "//ol", "govuk-list--number");
                ApplyClass(document, "//table", "govuk-table");
                ApplyClass(document, "//caption", "govuk-table__caption");
                ApplyClass(document, "//thead", "govuk-table__head");
                ApplyClass(document, "//tbody", "govuk-table__body");
                ApplyClass(document, "//tr", "govuk-table__row");
                ApplyClass(document, "//th", "govuk-table__header");
                ApplyClass(document, "//td", "govuk-table__cell");

                ApplyPermittedStylesToOrderedLists(document);

                if (options.RemoveWrappingParagraph)
                {
                    return RemoveWrappingParagraph(document);
                }
                else if (options.RemoveWrappingParagraphs)
                {
                    return RemoveWrappingParagraphs(document);
                }
                else
                {
                    return document.DocumentNode.OuterHtml;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// The Umbraco UI for ordered lists has a setting for selecting the style of the ordered list, so we need to enable it or it looks broken.
        /// However it is serialized to the style attribute which we don't want to allow free use of, so convert permitted style attribute values
        /// to classes for display, and remove any others.
        /// </summary>
        /// <param name="document"></param>
        private static void ApplyPermittedStylesToOrderedLists(HtmlDocument document)
        {
            var permittedStyleAttributes = new Dictionary<string, string> {
                {"list-style-type: lower-alpha;" , "govuk-list--lower-alpha" },
                {"list-style-type: lower-greek;" , "govuk-list--lower-greek" },
                {"list-style-type: lower-roman;" , "govuk-list--lower-roman" },
                {"list-style-type: upper-alpha;" , "govuk-list--upper-alpha" },
                {"list-style-type: upper-roman;" , "govuk-list--upper-roman" }
            };

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
            }
        }

        private static string RemoveWrappingParagraphs(HtmlDocument document)
        {
            var paragraphs = document.DocumentNode.Elements("p").ToList();
            for (var i = 0; i < paragraphs.Count; i++)
            {
                foreach (var child in paragraphs[i].ChildNodes)
                {
                    document.DocumentNode.InsertBefore(child, paragraphs[i]);
                }
                paragraphs[i].Remove();
            }
            return document.DocumentNode.OuterHtml;
        }

        /// <summary>
        /// TinyMCE automatically surrounds text in a paragraph. Remove that paragraph.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static string RemoveWrappingParagraph(HtmlDocument document)
        {
            if (document.DocumentNode.ChildNodes.Count == 1 &&
                document.DocumentNode.FirstChild.NodeType == HtmlNodeType.Element &&
                document.DocumentNode.FirstChild.Name == "p")
            {
                return document.DocumentNode.FirstChild.InnerHtml;
            }
            return document.DocumentNode.OuterHtml;
        }

        private static void ApplyInverseClasses(HtmlDocument document)
        {
            ApplyClass(document, "//a", "govuk-link--inverse");
        }

        private static void ApplyClass(HtmlDocument doc, string xpath, string className, IEnumerable<string>? unlessTheseClassesAreApplied = null)
        {
            var nodes = doc.DocumentNode.SelectNodes(xpath);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var addClass = true;
                    if (unlessTheseClassesAreApplied != null)
                    {
                        foreach (var unlessClass in unlessTheseClassesAreApplied)
                        {
                            if (node.HasClass(unlessClass))
                            {
                                addClass = false;
                                break;
                            }
                        }
                    }
                    if (addClass)
                    {
                        node.AddClass(className);
                    }
                }
            }
        }
    }
}
