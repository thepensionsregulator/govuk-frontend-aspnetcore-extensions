using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string DefaultSearchLabel = "Search Q&As";

        public TagBuilder GenerateTprSearchInput(string? searchLabel = null)
        {
            if (string.IsNullOrWhiteSpace(searchLabel))
            {
                searchLabel = DefaultSearchLabel;
            }

            var outer = new TagBuilder("div");
            outer.AddCssClass("govuk-grid-row");

            var button = new TagBuilder("button");
            button.AddCssClass("govuk-button");
            button.AddCssClass("tpr-button--no-next-step");
            button.Attributes.Add("id", "ask-button");
            button.InnerHtml.Append("Ask");

            var formGroup = new TagBuilder("div");
            formGroup.AddCssClass("govuk-form-group");

            var label = new TagBuilder("label");
            label.AddCssClass("govuk-label govuk-visually-hidden");
            label.InnerHtml.Append(searchLabel);

            var input = new TagBuilder("input");
            input.Attributes.Add("type", "text");
            input.Attributes.Add("id", "ask-input");
            input.AddCssClass("govuk-input");

            formGroup.InnerHtml.AppendHtml(label);
            formGroup.InnerHtml.AppendHtml(input);

            var heading = new TagBuilder("h2");
            heading.AddCssClass("govuk-heading-m");
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
