using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    public class TprSectionCardTitleTagHelper : TagHelper
    {
        internal const string TagName = "tpr-section-card-title";

        [HtmlAttributeName("href")]
        public string? Url { get; set; }

        [HtmlAttributeName("target")]
        public string? Target { get; set; }


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var cardContext = context.GetContextItem<TprSectionCardContext>();
            var content = await output.GetChildContentAsync();

            cardContext.SetTitle(output.Attributes.ToAttributeDictionary(),
                content,
                !content.IsEmptyOrWhiteSpace,
                Url,
                Target);

            output.SuppressOutput();
        }
    }
}
