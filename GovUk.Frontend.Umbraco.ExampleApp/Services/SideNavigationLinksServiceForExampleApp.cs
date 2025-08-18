using GovUk.Frontend.Umbraco.Models;
using GovUk.Frontend.Umbraco.Services;
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
            TprSideNavigationViewModel breadcrumbViewModel = new();
            breadcrumbViewModel.TitleLink = new TprSideNavigationLink { Name = "title", Url = "/"};
            breadcrumbViewModel.NavigationLinks = new List<TprSideNavigationLink>
            {
                new TprSideNavigationLink { Name = "First", Url = "/first"},
                new TprSideNavigationLink { Name = "title", Url = "/"},
                new TprSideNavigationLink { Name = "title", Url = "/"},
                new TprSideNavigationLink { Name = "title", Url = "/"},
                new TprSideNavigationLink { Name = "title", Url = "/"},
                new TprSideNavigationLink { Name = "title", Url = "/"},
                new TprSideNavigationLink { Name = "title", Url = "/"},
                new TprSideNavigationLink { Name = "title", Url = "/"},
                new TprSideNavigationLink { Name = "title", Url = "/"},
                new TprSideNavigationLink { Name = "title", Url = "/"},
                new TprSideNavigationLink { Name = "title", Url = "/"},
            };
            return breadcrumbViewModel;
        }
    }
}
