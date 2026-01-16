using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprAddressLookupTagHelper.TagName)]
    public class TprAddressLookupLegendTagHelper : TagHelper
    {
        internal const string TagName = "tpr-address-lookup-legend";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var addressLookupContext = context.GetContextItem<TprAddressLookupContext>();

            var childContent = await output.GetChildContentAsync();

            addressLookupContext.SetLegend(output.Attributes.ToAttributeDictionary(), childContent);

            output.SuppressOutput();
        }
    }
}
