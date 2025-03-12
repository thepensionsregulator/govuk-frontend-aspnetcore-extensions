using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprSectionCardsContainerTagHelper.TagName)]
    [RestrictChildren(TprSectionCardsCardTitleTagHelper.TagName, TprSectionCardsCardContentTagHelper.TagName)]

    public class TprSectionCardsCardTagHelper : TagHelper
    {
        internal const string TagName = "tpr-section-cards-card";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var sectionCardsContext = context.GetContextItem<TprSectionCardsContainerContext>();
            var cardContext = new TprSectionCardsCardContext
            {
                CardAttributes = output.Attributes.ToAttributeDictionary()
            };

            using (context.SetScopedContextItem(cardContext)) {
                await output.GetChildContentAsync();
            }
           
            sectionCardsContext.AddCard(cardContext);
            output.SuppressOutput();
        }
    }
}
