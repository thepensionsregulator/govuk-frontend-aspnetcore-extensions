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

            if (hasContext1)
            {
                var contextElement = new TagBuilder("div");
                contextElement.MergeCssClass("govuk-body");
                contextElement.MergeCssClass("tpr-context__context-1");
                contextElement.InnerHtml.Append(context1!);
                inner.InnerHtml.AppendHtml(contextElement);
            }

            if (hasContext2 || hasContext3)
            {
                var context23container = new TagBuilder("div");
                context23container.MergeCssClass("tpr-context__container");

                if (hasContext2)
                {
                    var contextElement = new TagBuilder("div");
                    contextElement.MergeCssClass("govuk-body");
                    contextElement.MergeCssClass("tpr-context__context-2");
                    contextElement.InnerHtml.Append(context2!);
                    context23container.InnerHtml.AppendHtml(contextElement);
                }

                if (hasContext3)
                {
                    var contextElement = new TagBuilder("div");
                    contextElement.MergeCssClass("govuk-body");
                    contextElement.MergeCssClass("tpr-context__context-3");

                    var innerContextElement = new TagBuilder("div");
                    innerContextElement.MergeCssClass("tpr-context__context-3-inner");
                    innerContextElement.InnerHtml.Append(context3!);
                    contextElement.InnerHtml.AppendHtml(innerContextElement);
                    context23container.InnerHtml.AppendHtml(contextElement);
                }

                inner.InnerHtml.AppendHtml(context23container);
            }

            tagBuilder.InnerHtml.AppendHtml(inner);

            return tagBuilder;
        }
    }
}
