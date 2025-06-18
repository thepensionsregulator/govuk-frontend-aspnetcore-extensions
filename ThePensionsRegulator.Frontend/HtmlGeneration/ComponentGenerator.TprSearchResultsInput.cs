using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.ObjectPool;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string DefaultSearchLabel = "Search Q&As";

        public const int SearchResultsMinHeadingLevel = 2;
        public const int SearchResultsMaxHeadingLevel = 6;

        //public const string[] 
        public static string[] AllHeadingClasses = ["govuk-heading-xl", "govuk-heading-l", "govuk-heading-m", "govuk-heading-s"];

        public TagBuilder GenerateTprSearchResultsInput(int headingLevel, string headingClass, string? searchLabel = null)
        {
            if (string.IsNullOrWhiteSpace(searchLabel))
            {
                searchLabel = DefaultSearchLabel;
            }

            var outer = new TagBuilder("div");

            var button = new TagBuilder("button");
            button.AddCssClass("govuk-button");
            button.AddCssClass("tpr-button--no-next-step");
            button.Attributes.Add("id", "ask-button");
            button.InnerHtml.Append("Ask");

            var formGroup = new TagBuilder("div");
            formGroup.AddCssClass("govuk-form-group");

            var label = new TagBuilder("label");
            label.AddCssClass("govuk-label govuk-visually-hidden");
            label.Attributes.Add("for", "ask-input");
            label.InnerHtml.Append(searchLabel);

            var input = new TagBuilder("input");
            input.Attributes.Add("type", "text");
            input.Attributes.Add("id", "search-results-ask-input");
            input.AddCssClass("govuk-input");

            formGroup.InnerHtml.AppendHtml(label);
            formGroup.InnerHtml.AppendHtml(input);

            var heading = new TagBuilder($"h{headingLevel}");
            heading.AddCssClass(headingClass);
            heading.AddCssClass("tpr-search-results__heading");
            heading.InnerHtml.Append(searchLabel);
            outer.InnerHtml.AppendHtml(heading);

            var inputGroup = new TagBuilder("div");
            inputGroup.AddCssClass("tpr-search-results__input");
            inputGroup.InnerHtml.AppendHtml(formGroup);

            inputGroup.InnerHtml.AppendHtml(button);

            outer.InnerHtml.AppendHtml(inputGroup);

            return outer;
        }
    }
}
