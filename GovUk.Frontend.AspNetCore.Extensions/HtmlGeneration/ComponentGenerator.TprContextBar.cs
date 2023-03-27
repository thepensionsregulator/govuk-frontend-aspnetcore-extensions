#nullable enable
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TprContextBarElement = "div";

        public virtual TagBuilder GenerateTprContextBar(TprContextBar tprContextBar)
        {
            var tagBuilder = new TagBuilder(TprContextBarElement);
            if (tprContextBar.ContextBarAttributes != null) tagBuilder.MergeAttributes(tprContextBar.ContextBarAttributes);
            tagBuilder.MergeCssClass("tpr-context");

            var inner = new TagBuilder("div");
            inner.MergeCssClass("govuk-width-container");
            inner.MergeCssClass("tpr-context__inner");

            var hasContext1 = !string.IsNullOrWhiteSpace(tprContextBar.Context1Content?.ToString());
            var hasContext2 = !string.IsNullOrWhiteSpace(tprContextBar.Context2Content?.ToString());
            var hasContext3 = !string.IsNullOrWhiteSpace(tprContextBar.Context3Content?.ToString());

            var context1Element = new TagBuilder("div");
            if (tprContextBar.Context1Attributes != null) { context1Element.MergeAttributes(tprContextBar.Context1Attributes); }
            context1Element.MergeCssClass("govuk-body");
            context1Element.MergeCssClass("tpr-context__context-1");
            if (hasContext1)
            {
                if (tprContextBar.Context1AllowHtml)
                {
                    context1Element.InnerHtml.AppendHtml(tprContextBar.Context1Content!);
                }
                else
                {
                    context1Element.InnerHtml.Append(tprContextBar.Context1Content!.ToString()!);
                }
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
            if (tprContextBar.Context2Attributes != null) { context2Element.MergeAttributes(tprContextBar.Context2Attributes); }
            context2Element.MergeCssClass("govuk-body");
            context2Element.MergeCssClass("tpr-context__context-2");
            if (hasContext2)
            {
                if (tprContextBar.Context2AllowHtml)
                {
                    var context2Inner = new TagBuilder("div");
                    context2Inner.InnerHtml.AppendHtml(tprContextBar.Context2Content!);
                    context2Element.InnerHtml.AppendHtml(context2Inner);
                }
                else
                {
                    context2Element.InnerHtml.Append(tprContextBar.Context2Content!.ToString()!);
                }
            }
            context23container.InnerHtml.AppendHtml(context2Element);

            if (hasContext3)
            {
                var context3Element = new TagBuilder("div");
                if (tprContextBar.Context3Attributes != null) { context3Element.MergeAttributes(tprContextBar.Context3Attributes); }
                context3Element.MergeCssClass("govuk-body");
                context3Element.MergeCssClass("tpr-context__context-3");

                var context3InnerElement = new TagBuilder("div");
                context3InnerElement.MergeCssClass("tpr-context__context-3-inner");
                if (tprContextBar.Context3AllowHtml)
                {
                    context3InnerElement.InnerHtml.AppendHtml(tprContextBar.Context3Content!);
                }
                else
                {
                    context3InnerElement.InnerHtml.Append(tprContextBar.Context3Content!.ToString()!);
                }
                context3Element.InnerHtml.AppendHtml(context3InnerElement);
                context23container.InnerHtml.AppendHtml(context3Element);
            }

            inner.InnerHtml.AppendHtml(context23container);

            tagBuilder.InnerHtml.AppendHtml(inner);

            return tagBuilder;
        }
    }
}
