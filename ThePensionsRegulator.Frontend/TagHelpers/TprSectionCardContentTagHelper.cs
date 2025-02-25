using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    public class TprSectionCardContentTagHelper : TagHelper
    {
        internal const string TagName = "tpr-section-card-content";
              
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var cardContext = context.GetContextItem<TprSectionCardContext>();
            var content = await output.GetChildContentAsync();

            cardContext.SetContent(              
                output.Attributes.ToAttributeDictionary(),             
                content,
                !content.IsEmptyOrWhiteSpace
             );

            output.SuppressOutput();
        }
    }
}
