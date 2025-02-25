using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    [RestrictChildren(TprSectionCardTitleTagHelper.TagName, TprSectionCardContentTagHelper.TagName)]

    public class TprSectionCardBodyTagHelper : TagHelper
    {
        internal const string TagName = "tpr-section-card-body";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            var cardContext = context.GetContextItem<TprSectionCardContext>();
            var childContent = await output.GetChildContentAsync();

            cardContext.SetBody(output.Attributes.ToAttributeDictionary(),               
                childContent,
                !childContent.IsEmptyOrWhiteSpace);

            output.SuppressOutput();
        }
    }
}