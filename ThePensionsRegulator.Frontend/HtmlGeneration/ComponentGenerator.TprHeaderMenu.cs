using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    partial class ComponentGenerator
    {
        public TagBuilder GenerateTprHeaderMenu(TprHeaderMenu tprMobileMenu, TprHeaderBar tprHeaderBar)
        {
            var mobileMenuContainer = new TagBuilder("div");
            mobileMenuContainer.AddCssClass("tpr-mobile-menu__container-inner");

            var icon = GenerateTprMobileMenuIcon();

            var mobileMenuLink = new TagBuilder("a");
            mobileMenuLink.AddCssClass("tpr-mobile-menu__no-js-link");
            mobileMenuLink.Attributes.Add("href", tprHeaderBar.MobileMenuNoJsNavPage);

            var noJsIcon = new TagBuilder("div");
            noJsIcon.AddCssClass("tpr-mobile-menu__icon-no-js");

            var mobileMenuLinkInner = new TagBuilder("span");
            mobileMenuLinkInner.AddCssClass("tpr-mobile-menu__no-js-link-inner");

            if (!string.IsNullOrEmpty((tprHeaderBar.HeaderMenuToggleClosed)))
            {
                mobileMenuLinkInner.Attributes.Add("data-close-label", tprHeaderBar.HeaderMenuToggleClosed);
                mobileMenuLinkInner.InnerHtml.Append(tprHeaderBar.HeaderMenuToggleClosed);
            }

            var mobileMenuToggle = new TagBuilder("button");
            mobileMenuToggle.AddCssClass("tpr-header-menu__button");
            mobileMenuToggle.AddCssClass("tpr-header-menu__button--hidden");
            mobileMenuToggle.Attributes.Add("aria-expanded", "false");

            if (tprMobileMenu.Attributes != null)
            {
                mobileMenuToggle.MergeAttributes(mobileMenuToggle.Attributes);
            }

            var closeButton = new TagBuilder("span");
            closeButton.AddCssClass("tpr-header-menu__button-inner");

            if (!string.IsNullOrEmpty(tprHeaderBar.HeaderMenuToggleOpen))
            {
                closeButton.Attributes.Add("data-open-label", tprHeaderBar.HeaderMenuToggleOpen);
            }
            if (!string.IsNullOrEmpty((tprHeaderBar.HeaderMenuToggleClosed)))
            {
                closeButton.Attributes.Add("data-close-label", tprHeaderBar.HeaderMenuToggleClosed);
                closeButton.InnerHtml.Append(tprHeaderBar.HeaderMenuToggleClosed);
            }

            mobileMenuToggle.InnerHtml.AppendHtml(icon);
            mobileMenuToggle.InnerHtml.AppendHtml(closeButton);

            mobileMenuLink.InnerHtml.AppendHtml(noJsIcon); //issue with before metric
            mobileMenuLink.InnerHtml.AppendHtml(mobileMenuLinkInner);

            mobileMenuContainer.InnerHtml.AppendHtml(mobileMenuLink);
            mobileMenuContainer.InnerHtml.AppendHtml(mobileMenuToggle);

            return mobileMenuContainer;
        }

        public TagBuilder GenerateTprMobileMenuIcon()
        {
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
            path1.AddCssClass("tpr-header-menu__menu-icon");
            svg2.InnerHtml.AppendHtml(path1);

            var path2 = new TagBuilder("path");
            path2.Attributes.Add("d", "M16,12.938 C16,13.456 15.58,13.876 15.062,13.876 L0.98,13.876 C0.462,13.876 0.042,13.456 0.042,12.938 L0.042,12.938 C0.042,12.42 0.462,12 0.98,12 L15.062,12 C15.58,12 16,12.42 16,12.938 L16,12.938 L16,12.938 Z");
            path2.AddCssClass("tpr-header-menu__menu-icon");
            svg2.InnerHtml.AppendHtml(path2);

            var path3 = new TagBuilder("path");
            path3.Attributes.Add("d", "M16,6.938 C16,7.456 15.58,7.876 15.062,7.876 L0.98,7.876 C0.462,7.876 0.042,7.456 0.042,6.938 L0.042,6.938 C0.042,6.42 0.462,6 0.98,6 L15.062,6 C15.58,6 16,6.42 16,6.938 L16,6.938 L16,6.938 Z");
            path3.AddCssClass("tpr-header-menu__menu-icon");
            svg2.InnerHtml.AppendHtml(path3);

            svg1.InnerHtml.AppendHtml(svg2);

            svgContainer.InnerHtml.AppendHtml(svg1);

            icon.InnerHtml.AppendHtml(svgContainer);

            return icon;
        }

        public TagBuilder GenerateTprHeaderNav(TprHeaderMenu headerMenu, TprHeaderBar tprHeaderBar)
        {
            var headerMenuNav = new TagBuilder("nav");
            headerMenuNav.AddCssClass("tpr-header-menu__nav-container");

            if (!string.IsNullOrWhiteSpace(tprHeaderBar.HeaderMenuAriaLabel))
            {
                headerMenuNav.Attributes.Add("aria-label", tprHeaderBar.HeaderMenuAriaLabel);
            }

            var tprWrapper = new TagBuilder("div");
            tprWrapper.AddCssClass("tpr-header-menu__nav-inner-container");
            headerMenuNav.InnerHtml.AppendHtml(tprWrapper);

            var govContainer = new TagBuilder("div");
            govContainer.AddCssClass("govuk-width-container");
            tprWrapper.InnerHtml.AppendHtml(govContainer);

            var navContainer = new TagBuilder("div");
            navContainer.AddCssClass("tpr-header-menu__nav");
            govContainer.InnerHtml.AppendHtml(navContainer);

            var headerMenuList = new TagBuilder("ul");
          
            navContainer.InnerHtml.AppendHtml(headerMenuList);

            if (tprHeaderBar.ShowSearch)
            {
                var searchContainer = new TagBuilder("li");
                searchContainer.AddCssClass("tpr-mobile-menu__header-search-container");
                headerMenuList.InnerHtml.AppendHtml(searchContainer);
                var tprHeaderSearch = GenerateTprHeaderSearch(tprHeaderBar);
                tprHeaderSearch.AddCssClass("tpr-header-search__mobile");
                searchContainer.InnerHtml.AppendHtml(tprHeaderSearch);
            }

            if (headerMenu.HeaderMenuItems != null)
            {
                var currentTprMenuItems = 6;

                foreach (var item in headerMenu.HeaderMenuItems)
                {
                    var mobileMenuItem = new TagBuilder("li");
                    
                    if (headerMenu.HeaderMenuItems.Count >= currentTprMenuItems || item != headerMenu.HeaderMenuItems.Last())
                    {
                        mobileMenuItem.AddCssClass("tpr-header-menu__nav-menu-item");
                    }
                    else 
                    {
                        mobileMenuItem.AddCssClass("tpr-header-menu__nav-menu-item tpr-header-menu__nav-final-item");
                    }
                    
                    headerMenuList.InnerHtml.AppendHtml(mobileMenuItem);

                    var arrowContainer = new TagBuilder("div");
                    arrowContainer.AddCssClass("tpr-header-menu__arrow-container");

                    var arrow = new TagBuilder("button");
                    arrow.AddCssClass("tpr-header-menu__arrow");
                    if (tprHeaderBar.HeaderMenuItemAriaLabel != null)
                    {
                        arrow.Attributes.Add("aria-label", $"{item.LinkText}: {tprHeaderBar.HeaderMenuItemAriaLabel}"); 
                    }
                    arrow.Attributes.Add("aria-expanded", "true");
                    arrowContainer.InnerHtml.AppendHtml(arrow);

                    var anchorTag = new TagBuilder("a");

                    if (item.Attributes != null)
                    {
                        anchorTag.MergeAttributes(item.Attributes);
                    }

                    if (!string.IsNullOrWhiteSpace(item.LinkUrl))
                    {
                        anchorTag.Attributes.Add("href", item.LinkUrl);
                    }
                   
                    anchorTag.Attributes.Add("tabindex", "0");
                   
                    mobileMenuItem.InnerHtml.AppendHtml(anchorTag);
                    

                    if (!string.IsNullOrWhiteSpace(item.LinkText))
                    {
                        anchorTag.InnerHtml.Append(item.LinkText);
                    }
                    if (item.HeaderMenuChildItems != null && item.HeaderMenuChildItems.Count > 0)
                    {
                        mobileMenuItem.InnerHtml.AppendHtml(arrowContainer);

                        var headerMenuSubMenu = new TagBuilder("ul");
                        headerMenuSubMenu.AddCssClass("tpr-header-menu__nav-sub-menu");
                        mobileMenuItem.InnerHtml.AppendHtml(headerMenuSubMenu);

                        foreach (var subMenuItem in item.HeaderMenuChildItems)
                        {
                            var mobileMenuSubMenuItem = new TagBuilder("li");

                            mobileMenuSubMenuItem.AddCssClass("tpr-header-menu__nav-sub-menu-item");
                            headerMenuSubMenu.InnerHtml.AppendHtml(mobileMenuSubMenuItem);

                            var mobileMenuSubMenuItemTitle = new TagBuilder("div");
                            mobileMenuSubMenuItemTitle.AddCssClass("tpr-header-menu__nav-sub-menu-item-title");
                            mobileMenuSubMenuItem.InnerHtml.AppendHtml(mobileMenuSubMenuItemTitle);

                            var aTag = new TagBuilder("a");

                            if (subMenuItem.Attributes != null)
                            {
                                aTag.MergeAttributes(subMenuItem.Attributes);
                            }
                            if (!string.IsNullOrWhiteSpace(subMenuItem.LinkUrl))
                            {
                                aTag.Attributes.Add("href", subMenuItem.LinkUrl);
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
            overlay.AddCssClass("tpr-header-menu__nav-overlay");
            headerMenuNav.InnerHtml.AppendHtml(overlay);

            return headerMenuNav;
        }
    }
}

