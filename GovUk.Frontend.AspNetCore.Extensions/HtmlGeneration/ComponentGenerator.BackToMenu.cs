#nullable enable
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string BackToMenuDefaultContent = "Back to menu";
        internal const string BackToMenuElement = "p";
        internal const string BackToMenuLinkElement = "a";
        internal const string BackToMenuDefaultHref = "#";

        public virtual TagBuilder GenerateBackToMenu(
            string href,
            IHtmlContent content,
            AttributeDictionary? attributes)
        {
            Guard.ArgumentNotNullOrEmpty(nameof(href), href);
            Guard.ArgumentNotNull(nameof(content), content);

            var tagBuilder = new TagBuilder(BackToMenuElement);
            if (attributes != null) tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-body");
            tagBuilder.MergeCssClass("govuk-back-to-menu");

            var linkBuilder = new TagBuilder(BackToMenuLinkElement);
            linkBuilder.MergeCssClass("govuk-link");
            linkBuilder.Attributes.Add("href", href);
            linkBuilder.InnerHtml.AppendHtml(content);

            using (var writer = new StringWriter())
            {
                linkBuilder.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                tagBuilder.InnerHtml.AppendHtml(writer.ToString()!);
            }

            return tagBuilder;
        }
    }
}
