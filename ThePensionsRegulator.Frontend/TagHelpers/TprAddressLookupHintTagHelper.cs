using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprAddressLookupTagHelper.TagName)]
    public class TprAddressLookupHintTagHelper : TagHelper
    {
        internal const string TagName = "tpr-address-lookup-hint";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var addressLookupContext = context.GetContextItem<TprAddressLookupContext>();

            var childContent = await output.GetChildContentAsync();

            addressLookupContext.SetHint(
                output.Attributes.ToAttributeDictionary(),
                childContent);

            output.SuppressOutput();
        }
    }
}
