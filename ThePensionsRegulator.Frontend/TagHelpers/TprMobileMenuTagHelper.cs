using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    public class TprMobileMenuTagHelper : TagHelper
    {
        private readonly ITprHtmlGenerator _htmlGenerator;

        internal const string TagName = "tpr-mobile-menu";

        public TprMobileMenuTagHelper()
          : this(htmlGenerator: null)
        {
        }

        internal TprMobileMenuTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var mobileMenuContext = new TprMobileMenuContext();

            using (context.SetScopedContextItem(mobileMenuContext))
            {
                await output.GetChildContentAsync();
            }

            var mobileMenu = new TprMobileMenu
            {
                Attributes = mobileMenuContext.Attributes,
                MobileMenuItems = mobileMenuContext.MenuItems.Select(i => new TprMobileMenuItem
                {
                    Attributes = i.Attributes,
                    LinkText = i.LinkText,
                    LinkDestination = i.LinkDestination,
                    Hierarchy = i.Hierarchy,
                    Placement = i.Placement,
                    SubMenuItems = i.SubMenuItems.Select(s => new TprMobileMenuSubMenuItem
                    {
                        LinkDestination = s.LinkDestination,
                        LinkText = s.LinkText,
                    }).ToList()

                }).ToList()
            };

            var tagBuilder = _htmlGenerator.GenerateTprMobileMenu(mobileMenu);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
