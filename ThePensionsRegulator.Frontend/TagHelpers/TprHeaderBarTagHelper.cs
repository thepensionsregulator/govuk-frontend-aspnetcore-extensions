using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    /// <summary>
    /// Generates a TPR header bar component
    /// </summary>
    [HtmlTargetElement(TagName)]
    [RestrictChildren(TprHeaderBarLogoTagHelper.TagName, TprHeaderBarLabelTagHelper.TagName, TprHeaderBarContentTagHelper.TagName, TprHeaderSearchTagHelper.TagName, TprHeaderMenuTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TprHeaderBarElement)]
    public class TprHeaderBarTagHelper : TagHelper
    {
        internal const string TagName = "tpr-header-bar";

        private readonly ITprHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="TprHeaderBarTagHelper"/>.
        /// </summary>
        public TprHeaderBarTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal TprHeaderBarTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var barContext = new TprHeaderBarContext();

            using (context.SetScopedContextItem(barContext))
            {
                await output.GetChildContentAsync();
            }

            List<TprHeaderMenuItem>? headerMenuItems = null;
            AttributeDictionary? headerMenuAttributes = null;

            if (barContext.TprHeaderMenuContext != null)
            {
                headerMenuAttributes = barContext.TprHeaderMenuContext.Attributes;
                headerMenuItems = barContext.TprHeaderMenuContext.HeaderMenuParentItems.Select(i => new TprHeaderMenuItem
                {
                    Attributes = i.Attributes,
                    LinkText = i.LinkText,
                    LinkUrl = i.LinkUrl,
                    HeaderMenuChildItems = i.HeaderMenuChildItems.Select(s => new TprHeaderMenuChildItem
                    {
                        LinkUrl = s.LinkUrl,
                        LinkText = s.LinkText,
                        Attributes = s.Attributes
                    }).ToList()
                }).ToList();
            }

            var tagBuilder = _htmlGenerator.GenerateTprHeaderBar(new TprHeaderBar
            {
                HeaderBarAttributes = output.Attributes.ToAttributeDictionary(),
                LogoAttributes = barContext.LogoAttributes,
                LogoHref = barContext.LogoHref ?? ComponentGenerator.HeaderLogoDefaultHref,
                LogoAlternativeText = barContext.LogoAlternativeText ?? ComponentGenerator.HeaderLogoDefaultAlt,
                LabelAttributes = barContext.LabelAttributes,
                Label = barContext.Label ?? new HtmlString(ComponentGenerator.DefaultHeaderLabel),
                LabelAllowHtml = barContext.LabelAllowHtml,
                ContentAttributes = barContext.ContentAttributes,
                Content = barContext.Content,
                ContentAllowHtml = barContext.ContentAllowHtml,
                ShowSearch = barContext.ShowSearch,
                SearchAttributes = barContext?.SearchAttributes,
                ActionPath = barContext?.ActionPath,
                AutoCompleteUrl = barContext?.AutoCompleteUrl,
                SearchPlaceholderText = barContext?.SearchPlaceholderText,
                SearchAriaLabel = barContext?.SearchAriaLabel,
                SearchInputName = barContext?.SearchInputName,
                DisplayHeaderMenu = barContext?.DisplayHeaderMenu,
                HeaderMenuAttributes = headerMenuAttributes,
                HeaderMenuItems = headerMenuItems,
                HeaderMenuAriaLabel = barContext?.HeaderMenuAriaLabel,
                HeaderMenuItemAriaLabel = barContext?.HeaderMenuItemAriaLabel,
                MobileMenuNoJsNavPage = barContext?.MobileMenuNoJsNavPage,
                HeaderMenuToggleClosed = barContext?.HeaderMenuToggleClosed,
                HeaderMenuToggleOpen = barContext?.HeaderMenuToggleOpen

            });

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
