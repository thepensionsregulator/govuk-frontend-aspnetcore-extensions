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
            var rootNode = currentPage.Root();
            TprSideNavigationViewModel sideNavigationViewModel = new();

            sideNavigationViewModel.TitleLink = new TprSideNavigationLink { Name = rootNode.Name, Url = rootNode.Url(), IsCurrentPage = (rootNode == currentPage) };
            sideNavigationViewModel.NavigationLinks = CreateSideNavigationLinkChildren(currentPage, null, rootNode.Children);
            return sideNavigationViewModel;
        }

        private List<TprSideNavigationLink> CreateSideNavigationLinkChildren(IPublishedContent currentPage, TprSideNavigationLink parent, IEnumerable<IPublishedContent> childPages)
        {
            List<TprSideNavigationLink> children = new();

            if (childPages is not null && childPages.Any())
            {
                foreach (var child in childPages)
                {
                    var childNavItem = new TprSideNavigationLink { 
                        Name = child.Name, 
                        Url = child.Url(), 
                        IsCurrentPage = child == currentPage, 
                        IsExpanded = (currentPage.Ancestors().Contains(child) || child == currentPage) && child.Level == 2, 
                        Parent = parent 
                    };
                    childNavItem.Children = CreateSideNavigationLinkChildren(currentPage, childNavItem, child.Children);
                    children.Add(childNavItem);
                }
            }
           return children;
        }
    }
}
