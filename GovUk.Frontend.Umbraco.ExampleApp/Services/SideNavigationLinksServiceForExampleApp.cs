using System.Collections.Generic;
using System.Linq;
using ThePensionsRegulator.Frontend.Umbraco.Models;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.ExampleApp.Services
{
    public class SideNavigationLinksServiceForExampleApp : ITprSideNavigationLinksService
    {
        public TprSideNavigationViewModel GetLinks(IPublishedContent currentPage)
        {
            var rootNode = currentPage.Root().Children.FirstOrDefault(x => x.Name == "Side navigation");
            TprSideNavigationViewModel sideNavigationViewModel = new();

            sideNavigationViewModel.TitleLink = new TprSideNavigationLink { Name = rootNode.Name, Url = rootNode.Url(), IsCurrentPage = (rootNode == currentPage) };
            sideNavigationViewModel.NavigationLinks = CreateSideNavigationLinkChildren(currentPage, null, rootNode);
            return sideNavigationViewModel;
        }

        private List<TprSideNavigationLink> CreateSideNavigationLinkChildren(IPublishedContent currentPage, TprSideNavigationLink parent, IPublishedContent rootNode)
        {
            List<TprSideNavigationLink> children = new();

            if (rootNode.Children is not null && rootNode.Children.Any())
            {
                foreach (var child in rootNode.Children)
                {
                    if ((parent is not null && (parent.IsExpanded || parent.IsCurrentPage)) || currentPage.Ancestors().Contains(child) || child.Level == 3 || currentPage.Level == child.Level)
                    {
                        var childNavItem = new TprSideNavigationLink
                        {
                            Name = child.Name,
                            Url = child.Url(),
                            IsCurrentPage = child == currentPage,
                            IsExpanded = (currentPage.Ancestors().Contains(child) || child == currentPage) && child.Level == 3,
                            Parent = parent
                        };
                        childNavItem.Children = CreateSideNavigationLinkChildren(currentPage, childNavItem, child);
                        children.Add(childNavItem);
                    }
                }
            }
           return children;
        }
    }
}
