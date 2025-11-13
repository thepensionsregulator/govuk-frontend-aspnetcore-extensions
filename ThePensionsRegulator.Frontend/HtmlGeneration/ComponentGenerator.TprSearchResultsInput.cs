using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string DefaultSearchLabel = "Search Q&As";

        public const int SearchResultsMinHeadingLevel = 2;
        public const int SearchResultsMaxHeadingLevel = 6;

        public static string[] AllHeadingClasses = ["govuk-heading-xl", "govuk-heading-l", "govuk-heading-m", "govuk-heading-s"];

        public TagBuilder GenerateTprSearchResultsInput(int headingLevel, string headingClass, string? searchLabel = null)
        {
            if (string.IsNullOrWhiteSpace(searchLabel))
            {
                searchLabel = DefaultSearchLabel;
            }

            var heading = new TagBuilder($"h{headingLevel}");
            heading.AddCssClass(headingClass);
            heading.AddCssClass("tpr-search-results__heading");
            heading.InnerHtml.Append(searchLabel);

            var label = new TagBuilder("label");
            label.AddCssClass("govuk-label govuk-visually-hidden");
            label.Attributes.Add("for", "tpr-search-results-ask-input");
            label.InnerHtml.Append(searchLabel);

            var errorTextSpan = new TagBuilder("span");
            errorTextSpan.AddCssClass("govuk-visually-hidden");
            errorTextSpan.InnerHtml.Append("Error:");

            var errorTextParagraph = new TagBuilder("p");
            errorTextParagraph.AddCssClass("govuk-error-message field-validation-error govuk-visually-hidden");
            errorTextParagraph.Attributes.Add("id", "tpr-search-results-error-text");
            errorTextParagraph.InnerHtml.AppendHtml(errorTextSpan);

            var input = new TagBuilder("input");
            input.Attributes.Add("type", "text");
            input.Attributes.Add("id", "tpr-search-results-ask-input");
            input.Attributes.Add("aria-describedby", "tpr-search-results-error-text");
            input.Attributes.Add("required", "");
            input.AddCssClass("govuk-input");

            var submitButton = new TagBuilder("button");
            submitButton.AddCssClass("govuk-button");
            submitButton.Attributes.Add("id", "tpr-search-results-ask-button");
            submitButton.Attributes.Add("type", "submit");
            submitButton.InnerHtml.Append("Ask");

            var resetButton = new TagBuilder("button");
            resetButton.AddCssClass("govuk-button govuk-button--secondary");
            resetButton.Attributes.Add("id", "tpr-search-results-reset-button");
            resetButton.Attributes.Add("type", "button");
            resetButton.InnerHtml.Append("Clear");

            var buttonGroup = new TagBuilder("div");
            buttonGroup.AddCssClass("govuk-button-group");
            buttonGroup.InnerHtml.AppendHtml(submitButton);
            buttonGroup.InnerHtml.AppendHtml(resetButton);

            var inputGroup = new TagBuilder("div");
            inputGroup.AddCssClass("tpr-search-results__input-group");
            inputGroup.InnerHtml.AppendHtml(input);
            inputGroup.InnerHtml.AppendHtml(buttonGroup);

            var formGroup = new TagBuilder("div");
            formGroup.AddCssClass("govuk-form-group");
            formGroup.InnerHtml.AppendHtml(label);
            formGroup.InnerHtml.AppendHtml(errorTextParagraph);
            formGroup.InnerHtml.AppendHtml(inputGroup);

            var resultsText = new TagBuilder("p");
            resultsText.AddCssClass("govuk-body govuk-visually-hidden");
            resultsText.Attributes.Add("id", "tpr-search-results-text");

            var form = new TagBuilder("form");
            form.AddCssClass("tpr-search-results__form");
            form.InnerHtml.AppendHtml(formGroup);
            form.InnerHtml.AppendHtml(resultsText);

            var outer = new TagBuilder("div");
            outer.InnerHtml.AppendHtml(heading);
            outer.InnerHtml.AppendHtml(form);

            return outer;
        }
    }
}
