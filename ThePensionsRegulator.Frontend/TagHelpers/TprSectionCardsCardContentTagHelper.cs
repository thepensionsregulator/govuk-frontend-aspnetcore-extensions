using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    public class TprSectionCardsCardContentTagHelper : TagHelper
    {
        internal const string TagName = "tpr-section-cards-card-content";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var cardContext = context.GetContextItem<TprSectionCardsCardContext>();
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
