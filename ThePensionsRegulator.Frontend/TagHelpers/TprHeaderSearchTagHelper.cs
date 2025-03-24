using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;


namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprHeaderBarTagHelper.TagName)]
    public class TprHeaderSearchTagHelper : TagHelper
    {

        internal const string TagName = "tpr-header-search";
           
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var headerSearchContext = context.GetContextItem<TprHeaderBarContext>();

            var content = await output.GetChildContentAsync();

            headerSearchContext.SetSearch(output.Attributes.ToAttributeDictionary(),content, !content.IsEmptyOrWhiteSpace, true);

            output.SuppressOutput();
        }
    }
}
