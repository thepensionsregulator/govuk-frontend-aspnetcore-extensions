using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        public virtual TagBuilder GenerateTprHeaderSearch(TprHeaderSearch tprHeaderSearch)
        {
            var divTag = new TagBuilder("div");
            if (tprHeaderSearch.SearchAttributes != null) { divTag.MergeAttributes(tprHeaderSearch.SearchAttributes); }
            divTag.AddCssClass("search");

            var form = new TagBuilder("form");
            form.Attributes.Add("action", "/en/search-results");
            form.Attributes.Add("id", "form-globalsearch");
            form.Attributes.Add("method", "get");

            var formLabel = new TagBuilder("label");
            formLabel.Attributes.Add("for", "GlobalSearchInputBox");
            if (tprHeaderSearch.SearchBoxPrompt != null)
            {
                if (tprHeaderSearch.SearchBoxAllowHtml)
                {
                    formLabel.InnerHtml.AppendHtml(tprHeaderSearch.SearchBoxPrompt);
                }
                else
                {
                    formLabel.InnerHtml.Append(tprHeaderSearch.SearchBoxPrompt.ToString()!);
                }
            }

            form.InnerHtml.AppendHtml(formLabel);

            var searchField = new TagBuilder("div");
            searchField.AddCssClass("searchFieldWithButton");

            //var autoComplete = new TagBuilder("div");
            //autoComplete.AddCssClass("easy-autocomplete");
            //autoComplete.Attributes.Add("style", "width: 313.948px;");

            var input = new TagBuilder("input");
            input.Attributes.Add("aria-label", "Search");
            input.Attributes.Add("autocomplete", "off");
            input.AddCssClass("searchbox");
            input.Attributes.Add("id", "GlobalSearchInputBox");
            input.Attributes.Add("name", "query");
            input.Attributes.Add("placeholder", "Search");
            input.Attributes.Add("type", "text");
            input.Attributes.Add("value", "");

            searchField.InnerHtml.AppendHtml(input);

            //var autoCompleteContainer = new TagBuilder("div");
            //autoCompleteContainer.AddCssClass("easy-autocomplete-container");
            //autoCompleteContainer.Attributes.Add("id", "eac-container-GlobalSearchInputBox");

            //var autoCompleteResultContainer = new TagBuilder("ul");

            var button = new TagBuilder("button");
            button.Attributes.Add("type", "submit");
            button.Attributes.Add("class", "searchButton");
            button.Attributes.Add("aria-label", "Search");

            var buttonSvg = new TagBuilder("svg");
            //TODO: inner html for image
            button.InnerHtml.AppendHtml(buttonSvg);

            searchField.InnerHtml.AppendHtml(button);

            form.InnerHtml.AppendHtml(searchField);

            divTag.InnerHtml.AppendHtml(form);

            return divTag;
        }
    }
}
