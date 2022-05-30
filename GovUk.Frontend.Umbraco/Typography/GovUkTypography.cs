using HtmlAgilityPack;

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
                ApplyClass(document, "//h2", "govuk-heading-m");
                ApplyClass(document, "//h3", "govuk-heading-s");
                ApplyClass(document, "//p", "govuk-body");
                ApplyClass(document, "//ul", "govuk-list");
                ApplyClass(document, "//ul", "govuk-list--bullet");
                ApplyClass(document, "//ol", "govuk-list");
                ApplyClass(document, "//ol", "govuk-list--number");

                if (options.RemoveWrappingParagraph)
                {
                    return RemoveWrappingParagraph(document);
                }
                else
                {
                    return document.DocumentNode.OuterHtml;
                }
            }
            return string.Empty;
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

        private static void ApplyClass(HtmlDocument doc, string xpath, string className)
        {
            var nodes = doc.DocumentNode.SelectNodes(xpath);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    node.AddClass(className);
                }
            }
        }
    }
}
