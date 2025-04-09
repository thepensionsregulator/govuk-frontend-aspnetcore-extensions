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

        [HtmlAttributeName(ActionAttributeName)]
        public string? ActionPath { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var headerSearchContext = context.GetContextItem<TprHeaderBarContext>();

            var content = await output.GetChildContentAsync();

            headerSearchContext.SetSearch(output.Attributes.ToAttributeDictionary(), true, ActionPath);
           
            output.SuppressOutput();
        }
    }
}
