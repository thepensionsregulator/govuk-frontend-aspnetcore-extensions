using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using ThePensionsRegulator.GovUk.Frontend;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprRelatedLinksTagHelper.TagName)]
    public class TprRelatedLinksHeadingTagHelper : TagHelper
    {
        internal const string TagName = "tpr-related-links-heading";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var relatedLinksContext = context.GetContextItem<TprRelatedLinksContext>();

            var childContent = await output.GetChildContentAsync();

            relatedLinksContext.SetHeading(output.Attributes.ToAttributeDictionary(), childContent.Snapshot());

            output.SuppressOutput();
        }
    }
}
