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
                    ApplyClass(document, "//a", "govuk-link--inverse");
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
