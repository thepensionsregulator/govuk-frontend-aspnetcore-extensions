using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprHeaderBarTagHelper.TagName)]
    public class TprHeaderSearchTagHelper : TagHelper
    {
        internal const string TagName = "tpr-header-search";
        private const string ActionAttributeName = "action";
        private const string AutocompleteUrlAttribute = "autocomplete-url";
        private const string PlaceholderAttribute = "placeholder";

        [HtmlAttributeName(ActionAttributeName)]
        public string? ActionPath { get; set; }

        [HtmlAttributeName(AutocompleteUrlAttribute)]
        public string? AutocompleteUrl { get; set; }

        [HtmlAttributeName(PlaceholderAttribute)]    
        public string? PlaceholderText {  get; set; }

        public string? AriaLabel {  get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var headerSearchContext = context.GetContextItem<TprHeaderBarContext>();

            var content = await output.GetChildContentAsync();

            headerSearchContext.SetSearch(output.Attributes.ToAttributeDictionary(), true, ActionPath, AutocompleteUrl, PlaceholderText, AriaLabel);
           
            output.SuppressOutput();
        }
    }
}
