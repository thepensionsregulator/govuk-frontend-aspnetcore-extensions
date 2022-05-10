using HtmlAgilityPack;

namespace GovUk.Frontend.Umbraco.Html
{
    public static class GovUkHtmlHelper
    {
        /// <summary>
        /// TinyMCE automatically surrounds text in a paragraph. Remove that paragraph.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveWrappingParagraph(string html)
        {
            if (string.IsNullOrEmpty(html)) { return html; }
            var document = new HtmlDocument();
            document.LoadHtml(html);
            if (document.DocumentNode.FirstChild != null &&
                document.DocumentNode.FirstChild.NodeType == HtmlNodeType.Element &&
                document.DocumentNode.FirstChild.Name == "p")
            {
                return document.DocumentNode.FirstChild.InnerHtml;
            }
            return html;
        }
    }
}
