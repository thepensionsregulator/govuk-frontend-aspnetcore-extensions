using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprSectionCardsTagHelper.TagName)]
    [RestrictChildren(TprSectionCardTitleTagHelper.TagName, TprSectionCardContentTagHelper.TagName)]

    public class TprSectionCardTagHelper : TagHelper
    {
        internal const string TagName = "tpr-section-card";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var sectionCardsContext = context.GetContextItem<TprSectionCardsContext>();
            var cardContext = new TprSectionCardContext
            {
                CardAttributes = output.Attributes.ToAttributeDictionary()
            };

            using (context.SetScopedContextItem(cardContext))
            {
                await output.GetChildContentAsync();
            }

            sectionCardsContext.AddCard(cardContext);
            output.SuppressOutput();
        }
    }
}
