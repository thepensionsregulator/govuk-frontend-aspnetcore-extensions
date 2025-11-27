using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    public class TprFooterBarThreeColumnLinksTagHelper : TagHelper
    {
        internal const string TagName = "tpr-footer-bar-three-column-links";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var threeColumnLinksContext = new TprFooterBarThreeColumnLinksContext();

            using (context.SetScopedContextItem(threeColumnLinksContext)) {

                await output.GetChildContentAsync();                
            }

            var footerContext = context.GetContextItem<TprFooterBarContext>();

            footerContext.AddThreeColumLinks(threeColumnLinksContext);
            

            output.SuppressOutput();
        }

    }
}
