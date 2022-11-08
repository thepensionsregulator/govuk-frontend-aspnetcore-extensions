#nullable enable
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TprContextBarElement = "div";

        public virtual TagBuilder GenerateTprContextBar(
            string? context1,
            string? context2,
            string? context3,
            AttributeDictionary? attributes)
        {
            var tagBuilder = new TagBuilder(TprContextBarElement);
            if (attributes != null) tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("tpr-context");

            var inner = new TagBuilder("div");
            inner.MergeCssClass("govuk-width-container");
            inner.MergeCssClass("tpr-context__inner");

            var hasContext1 = !string.IsNullOrWhiteSpace(context1);
            var hasContext2 = !string.IsNullOrWhiteSpace(context2);
            var hasContext3 = !string.IsNullOrWhiteSpace(context3);

            var context1Element = new TagBuilder("div");
            context1Element.MergeCssClass("govuk-body");
            context1Element.MergeCssClass("tpr-context__context-1");
            if (hasContext1)
            {
                context1Element.InnerHtml.Append(context1!);
            }
            inner.InnerHtml.AppendHtml(context1Element);

            var context23container = new TagBuilder("div");
            context23container.MergeCssClass("tpr-context__container");
            if (!hasContext2)
            {
                context23container.MergeCssClass("tpr-context__container--context-2-empty");
            }
            if (!hasContext3)
            {
                context23container.MergeCssClass("tpr-context__container--context-3-empty");
            }

            var context2Element = new TagBuilder("div");
            context2Element.MergeCssClass("govuk-body");
            context2Element.MergeCssClass("tpr-context__context-2");
            if (hasContext2)
            {
                context2Element.InnerHtml.Append(context2!);
            }
            context23container.InnerHtml.AppendHtml(context2Element);

            if (hasContext3)
            {
                var context3Element = new TagBuilder("div");
                context3Element.MergeCssClass("govuk-body");
                context3Element.MergeCssClass("tpr-context__context-3");

                var context3InnerElement = new TagBuilder("div");
                context3InnerElement.MergeCssClass("tpr-context__context-3-inner");
                context3InnerElement.InnerHtml.Append(context3!);
                context3Element.InnerHtml.AppendHtml(context3InnerElement);
                context23container.InnerHtml.AppendHtml(context3Element);
            }

            inner.InnerHtml.AppendHtml(context23container);

            tagBuilder.InnerHtml.AppendHtml(inner);

            return tagBuilder;
        }
    }
}
