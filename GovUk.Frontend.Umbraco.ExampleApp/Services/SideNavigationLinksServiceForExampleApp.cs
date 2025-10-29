using System;
using System.Collections.Generic;
using System.Linq;
using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Services;
using ThePensionsRegulator.Umbraco;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.ExampleApp.Services
{
    public class SideNavigationLinksServiceForExampleApp : ITprSideNavigationLinksService
    {
        private IUmbracoPublishedContentAccessor _publishedContext;

        public SideNavigationLinksServiceForExampleApp(IUmbracoPublishedContentAccessor publishedContext)
        {
            _publishedContext = publishedContext;
        }

        public TprSideNavigationViewModel GetLinks()
        {
            var currentPage = _publishedContext.PublishedContent ?? throw new ArgumentNullException(nameof(_publishedContext.PublishedContent), "Published context is not initialised.");
            var rootNode = currentPage.Root().Children().FirstOrDefault(x => x.Name == "Side navigation") ?? currentPage.Root();
            TprSideNavigationViewModel sideNavigationViewModel = new() {
                TitleLink = new TprSideNavigationLink { Name = rootNode.Name, Url = rootNode.Url(), IsCurrentPage = (rootNode == currentPage) },
                NavigationLinks = CreateSideNavigationLinkChildren(currentPage, null, rootNode)
            };
            return sideNavigationViewModel;
        }

        private List<TprSideNavigationLink> CreateSideNavigationLinkChildren(IPublishedContent currentPage, TprSideNavigationLink? parent, IPublishedContent rootNode)
        {
            List<TprSideNavigationLink> children = new();

            if (rootNode.Children() is not null && rootNode.Children().Any())
            {
                foreach (var child in rootNode.Children())
                {
                    var childNavItem = new TprSideNavigationLink
                    {
                        Name = child.Name,
                        Url = child.Url(),
                        IsCurrentPage = child == currentPage,
                        IsExpanded = currentPage.Ancestors().Contains(child) || child == currentPage,
                        Parent = parent
                    };
                    childNavItem.Children = CreateSideNavigationLinkChildren(currentPage, childNavItem, child);
                    children.Add(childNavItem);
                }
            }
           return children;
        }
    }
}
