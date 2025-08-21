using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    partial class ComponentGenerator
    {
        public TagBuilder GenerateTprHeaderMenu(TprHeaderMenu tprMobileMenu, TprHeaderBar tprHeaderBar)
        {

            var mobileMenuToggle = new TagBuilder("a");
            mobileMenuToggle.AddCssClass("tpr-mobile-menu-toggle");
            mobileMenuToggle.Attributes.Add("href", tprHeaderBar.MobileMenuNoJsNavPage);

            if (tprMobileMenu.Attributes != null)
            {
                mobileMenuToggle.MergeAttributes(mobileMenuToggle.Attributes);
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
            closeButton.InnerHtml.Append("Menu");

            svg1.InnerHtml.AppendHtml(svg2);

            svgContainer.InnerHtml.AppendHtml(svg1);

            icon.InnerHtml.AppendHtml(svgContainer);

            mobileMenuToggle.InnerHtml.AppendHtml(icon);
            mobileMenuToggle.InnerHtml.AppendHtml(closeButton);

            return mobileMenuToggle;
        }

        public TagBuilder GenerateTprHeaderNav(TprHeaderMenu headerMenu, TprHeaderBar tprHeaderBar)
        {
            var headerMenuNav = new TagBuilder("nav");
            headerMenuNav.AddCssClass("tpr-header-menu-nav");

            var tprWrapper = new TagBuilder("div");
            tprWrapper.AddCssClass("tpr-header-menu-nav__inner-container");
            headerMenuNav.InnerHtml.AppendHtml(tprWrapper);

            var govContainer = new TagBuilder("div");
            govContainer.AddCssClass("govuk-width-container");
            tprWrapper.InnerHtml.AppendHtml(govContainer);

            var navContainer = new TagBuilder("div");
            navContainer.AddCssClass("navigation");
            govContainer.InnerHtml.AppendHtml(navContainer);

            var headerMenuList = new TagBuilder("ul");

            if (!string.IsNullOrWhiteSpace(tprHeaderBar.HeaderMenuAriaLabel))
            {
                headerMenuList.Attributes.Add("aria-label", tprHeaderBar.HeaderMenuAriaLabel);
            }
            navContainer.InnerHtml.AppendHtml(headerMenuList);

            if (tprHeaderBar.ShowSearch)
            {
                var searchContainer = new TagBuilder("li");
                searchContainer.AddCssClass("tpr-mobile-menu__header-search-container");
                headerMenuList.InnerHtml.AppendHtml(searchContainer);
                var tprHeaderSearch = GenerateTprHeaderSearch(tprHeaderBar);
                tprHeaderSearch.AddCssClass("mobile");
                searchContainer.InnerHtml.AppendHtml(tprHeaderSearch);
            }

            if (headerMenu.HeaderMenuParentItems != null)
            {
                foreach (var item in headerMenu.HeaderMenuParentItems)
                {
                    var mobileMenuItem = new TagBuilder("li");

                    if (item.Attributes != null)
                    {
                        mobileMenuItem.MergeAttributes(item.Attributes);
                    }

                    if (item != headerMenu.HeaderMenuParentItems.Last())
                    {
                        mobileMenuItem.AddCssClass("navigation__menu-item");
                    }
                    else
                    {
                        mobileMenuItem.AddCssClass("navigation__menu-item navigation__final-item");
                    }

                    headerMenuList.InnerHtml.AppendHtml(mobileMenuItem);

                    var arrowContainer = new TagBuilder("div");
                    arrowContainer.AddCssClass("arrow_container");

                    var arrow = new TagBuilder("i");
                    arrow.AddCssClass("arrow right");

                    arrowContainer.InnerHtml.AppendHtml(arrow);

                    mobileMenuItem.InnerHtml.AppendHtml(arrowContainer);

                    var anchorTag = new TagBuilder("a");

                    if (!string.IsNullOrWhiteSpace(item.LinkDestination))
                    {
                        anchorTag.Attributes.Add("href", item.LinkDestination);
                    }

                    anchorTag.Attributes.Add("aria-expanded", "true");
                    anchorTag.Attributes.Add("tabindex", "0");
                    anchorTag.Attributes.Add("role", "button");

                    mobileMenuItem.InnerHtml.AppendHtml(anchorTag);

                    if (!string.IsNullOrWhiteSpace(item.LinkText))
                    {
                        anchorTag.InnerHtml.Append(item.LinkText);
                    }

                    var headerMenuSubMenu = new TagBuilder("div");
                    headerMenuSubMenu.AddCssClass("navigation__sub-menu");
                    mobileMenuItem.InnerHtml.AppendHtml(headerMenuSubMenu);

                    if (item.HeaderMenuChildItems != null)
                    {
                        foreach (var subMenuItem in item.HeaderMenuChildItems)
                        {
                            var mobileMenuSubMenuItem = new TagBuilder("div");

                            if (subMenuItem.Attriubutes != null)
                            {
                                mobileMenuItem.MergeAttributes(subMenuItem.Attriubutes);
                            }

                            mobileMenuSubMenuItem.AddCssClass("navigation__sub-menu-item");
                            headerMenuSubMenu.InnerHtml.AppendHtml(mobileMenuSubMenuItem);

                            var mobileMenuSubMenuItemTitle = new TagBuilder("div");
                            mobileMenuSubMenuItemTitle.AddCssClass("navigation__sub-menu-item-title");
                            mobileMenuSubMenuItem.InnerHtml.AppendHtml(mobileMenuSubMenuItemTitle);

                            var aTag = new TagBuilder("a");

                            if (!string.IsNullOrWhiteSpace(subMenuItem.LinkDestination))
                            {
                                aTag.Attributes.Add("href", subMenuItem.LinkDestination);
                            }

                            aTag.Attributes.Add("tabindex", "0");
                            mobileMenuSubMenuItemTitle.InnerHtml.AppendHtml(aTag);

                            if (!string.IsNullOrWhiteSpace(subMenuItem.LinkText))
                            {
                                aTag.InnerHtml.Append(subMenuItem.LinkText);
                            }
                        }
                    }
                }
            }

            var overlay = new TagBuilder("div");
            overlay.AddCssClass("nav-overlay");
            headerMenuNav.InnerHtml.AppendHtml(overlay);

            return headerMenuNav;
        }
    }
}

