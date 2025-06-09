using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    partial class ComponentGenerator
    {
        public TagBuilder GenerateTprMobileMenu(TprMobileMenu tprMobileMenu)
        {
            var mobileMenuDiv = new TagBuilder("div");
            mobileMenuDiv.AddCssClass("tpr-mobile-menu-toggle");

            if (tprMobileMenu.Attributes != null)
            {
                mobileMenuDiv.MergeAttributes(mobileMenuDiv.Attributes);
            }

            var icon = new TagBuilder("div");
            icon.AddCssClass("tpr-mobile-menu__icon");

            var svgContainer = new TagBuilder("svg");
            svgContainer.Attributes.Add("viewBox", "0 0 17 16");
            svgContainer.Attributes.Add("xmlns", "http://www.w3.org/2000/svg");
            svgContainer.Attributes.Add("version", "1.1");
            svgContainer.Attributes.Add("focusable", "false");
            svgContainer.AddCssClass("tpr-mobile-menu__svg");

            var svg1 = new TagBuilder("g");
            svg1.Attributes.Add("stroke", "none");
            svg1.Attributes.Add("stroke-width", "1");
            svg1.Attributes.Add("fill", "none");
            svg1.Attributes.Add("fill-rule", "evenodd");

            var svg2 = new TagBuilder("g");
            svg2.Attributes.Add("transform", "translate(1.000000, 1.000000)");
            svg2.Attributes.Add("fill", "#434343");

            var path1 = new TagBuilder("path");
            path1.Attributes.Add("d", "M16,0.938 C16,1.456 15.58,1.876 15.062,1.876 L0.98,1.876 C0.462,1.876 0.042,1.456 0.042,0.938 L0.042,0.938 C0.042,0.42 0.462,0 0.98,0 L15.062,0 C15.58,0 16,0.42 16,0.938 L16,0.938 L16,0.938 Z");
            path1.AddCssClass("si-glyph-fill");
            svg2.InnerHtml.AppendHtml(path1);

            var path2 = new TagBuilder("path");
            path2.Attributes.Add("d", "M16,12.938 C16,13.456 15.58,13.876 15.062,13.876 L0.98,13.876 C0.462,13.876 0.042,13.456 0.042,12.938 L0.042,12.938 C0.042,12.42 0.462,12 0.98,12 L15.062,12 C15.58,12 16,12.42 16,12.938 L16,12.938 L16,12.938 Z");
            path2.AddCssClass("si-glyph-fill");
            svg2.InnerHtml.AppendHtml(path2);

            var path3 = new TagBuilder("path");
            path3.Attributes.Add("d", "M16,6.938 C16,7.456 15.58,7.876 15.062,7.876 L0.98,7.876 C0.462,7.876 0.042,7.456 0.042,6.938 L0.042,6.938 C0.042,6.42 0.462,6 0.98,6 L15.062,6 C15.58,6 16,6.42 16,6.938 L16,6.938 L16,6.938 Z");
            path3.AddCssClass("si-glyph-fill");
            svg2.InnerHtml.AppendHtml(path3);

            var closeButton = new TagBuilder("button");
            closeButton.AddCssClass("closed");
            closeButton.Attributes.Add("aria-haspopup", "true");

            var openButton = new TagBuilder("button");
            openButton.AddCssClass("open");
            closeButton.Attributes.Add("aria-label", "Close menu");

            svg1.InnerHtml.AppendHtml(svg2);

            svgContainer.InnerHtml.AppendHtml(svg1);

            icon.InnerHtml.AppendHtml(svgContainer);

            mobileMenuDiv.InnerHtml.AppendHtml(icon);
            mobileMenuDiv.InnerHtml.AppendHtml(closeButton);
            mobileMenuDiv.InnerHtml.AppendHtml(openButton);

            var nav = GenerateTprMobileMenuNav(tprMobileMenu);
            mobileMenuDiv.InnerHtml.AppendHtml(nav);

            return mobileMenuDiv;
        }

        public TagBuilder GenerateTprMobileMenuNav(TprMobileMenu mobileMenu)
        {
            var mobileMenuNav = new TagBuilder("nav");
            mobileMenuNav.AddCssClass("mobileactive");

            var tprWrapper = new TagBuilder("div");
            tprWrapper.AddCssClass("tpr-main-wrapper");
            mobileMenuNav.InnerHtml.AppendHtml(tprWrapper);

            var govContainer = new TagBuilder("div");
            govContainer.AddCssClass("govuk-width-container");
            tprWrapper.InnerHtml.AppendHtml(govContainer);

            var navContainer = new TagBuilder("div");
            navContainer.AddCssClass("navigation");
            govContainer.InnerHtml.AppendHtml(navContainer);

            var mobileMenuList = new TagBuilder("ul");
            mobileMenuList.Attributes.Add("aria-label", "Use alt + down arrow to open menu and alt + up arrow to close menu."); 
            navContainer.InnerHtml.AppendHtml(mobileMenuList);

            if (mobileMenu.MobileMenuItems != null)
            {
                foreach (var item in mobileMenu.MobileMenuItems)
                {
                    var mobileMenuItem = new TagBuilder("li");

                    if(item.Attributes != null)
                    {
                        mobileMenuItem.MergeAttributes(item.Attributes);
                    }

                    if (item != mobileMenu.MobileMenuItems.Last())
                    {
                        mobileMenuItem.AddCssClass("navigation__menu-item");
                    }
                    else
                    {
                        mobileMenuItem.AddCssClass("navigation__menu-item navigation__final-item");
                    }

                    mobileMenuList.InnerHtml.AppendHtml(mobileMenuItem);

                    var anchorTag = new TagBuilder("a");
                    anchorTag.Attributes.Add("id", item.Id);

                    if (!string.IsNullOrWhiteSpace(item.LinkDestination))
                    {
                        anchorTag.Attributes.Add("href", item.LinkDestination);
                    }

                    anchorTag.Attributes.Add("aria-expanded", "false");
                    mobileMenuItem.InnerHtml.AppendHtml(anchorTag);

                    if (!string.IsNullOrWhiteSpace(item.LinkText))
                    {
                        anchorTag.InnerHtml.Append(item.LinkText);
                    }

                    var mobileMenuSubMenu = new TagBuilder("div");
                    mobileMenuSubMenu.AddCssClass("navigation__sub-menu");
                    mobileMenuSubMenu.Attributes.Add("aria-hidden", "true");

                    mobileMenuItem.InnerHtml.AppendHtml(mobileMenuSubMenu);

                    foreach (var subMenuItem in item.SubMenuItems)
                    {
                        var mobileMenuSubMenuItem = new TagBuilder("div");

                        if (subMenuItem.Attriubutes != null)
                        {
                            mobileMenuItem.MergeAttributes(subMenuItem.Attriubutes);
                        }

                        mobileMenuSubMenuItem.AddCssClass("navigation__sub-menu-item");
                        mobileMenuSubMenu.InnerHtml.AppendHtml(mobileMenuSubMenuItem);

                        var mobileMenuSubMenuItemTitle = new TagBuilder("div");
                        mobileMenuSubMenuItemTitle.AddCssClass("navigation__sub-menu-item-title");
                        mobileMenuItem.InnerHtml.AppendHtml(mobileMenuSubMenuItemTitle);

                        var aTag = new TagBuilder("a");
                        aTag.Attributes.Add("id", subMenuItem.Id);

                        if (!string.IsNullOrWhiteSpace(subMenuItem.LinkDestination))
                        {
                            aTag.Attributes.Add("href", subMenuItem.LinkDestination);
                        }

                        aTag.Attributes.Add("tabindex", "-1");
                        mobileMenuSubMenuItemTitle.InnerHtml.AppendHtml(aTag);

                        if (!string.IsNullOrWhiteSpace(subMenuItem.LinkText))
                        {
                            anchorTag.InnerHtml.Append(subMenuItem.LinkText);
                        }
                    }
                }
            }
            return mobileMenuNav;
        }
    }
}

