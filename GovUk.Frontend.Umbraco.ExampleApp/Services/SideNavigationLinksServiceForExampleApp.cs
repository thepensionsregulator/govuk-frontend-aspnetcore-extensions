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
            foreach (var child in rootNode.Children)
            {
                sideNavigationViewModel.NavigationLinks.Add(new TprSideNavigationLink { Name = child.Name, Url = child.Url() });
            }
            sideNavigationViewModel.TitleLink = new TprSideNavigationLink { Name = rootNode.Name, Url = rootNode.Url()};
            return sideNavigationViewModel;
        }
    }
}
