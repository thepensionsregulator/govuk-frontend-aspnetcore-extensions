using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    public class TprSectionCardsCardTitleTagHelper : TagHelper
    {
        internal const string TagName = "tpr-section-cards-card-title";

        [HtmlAttributeName("href")]
        public string? Url { get; set; }

        [HtmlAttributeName("target")]
        public string? Target { get; set; }


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var cardContext = context.GetContextItem<TprSectionCardsCardContext>();
            var content = await output.GetChildContentAsync();

            cardContext.SetTitle(output.Attributes.ToAttributeDictionary(),
                content,
                !content.IsEmptyOrWhiteSpace,
                Url!,
                Target!);

            output.SuppressOutput();
        }
    }
}
