using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using ThePensionsRegulator.GovUk.Frontend;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprHeaderMenuItemTagHelper.TagName)]
    public class TprHeaderMenuChildItemTagHelper : TagHelper
    {
        internal const string TagName = "tpr-header-menu-child-item";

        private const string UrlAttributeName = "href";
        private const string LinkTextAttributeName = "link-text";
        
        [HtmlAttributeName(UrlAttributeName)] 
        public required string Url { get; set; }

        [HtmlAttributeName(LinkTextAttributeName)]
        public required string LinkText { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var headerMenuItemContext = context.GetContextItem<TprHeaderMenuItemContext>();
            var headerMenuChildItemContext = new TprHeaderMenuChildItemContext
            {
                Attributes = output.Attributes.ToAttributeDictionary(),              
            };

            using (context.SetScopedContextItem(headerMenuChildItemContext))
            {
                await output.GetChildContentAsync();
            }
            
            headerMenuChildItemContext.SetChildItem(headerMenuChildItemContext.Attributes, LinkText, Url);
            headerMenuItemContext.AddChildItem(headerMenuChildItemContext);
            
            output.TagName = TagName;
            output.SuppressOutput();
        }
    }
}
