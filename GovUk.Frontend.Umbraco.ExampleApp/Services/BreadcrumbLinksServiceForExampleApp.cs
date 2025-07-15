using System.Linq;
using GovUk.Frontend.Umbraco.Models;
using GovUk.Frontend.Umbraco.Services;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.ExampleApp.Services
{
    public class BreadcrumbLinksServiceForExampleApp : IGovUkBreadcrumbLinksService
    {
        public BreadcrumbViewModel GetLinks(IPublishedContent page)
        {
            BreadcrumbViewModel breadcrumbViewModel = new();
            foreach (var ancestor in page.Ancestors().OrderBy(x => x.Level)) 
            {
                breadcrumbViewModel.Links.Add(new BreadcrumbLink { Name = ancestor.Name, Url = ancestor.Url() }); 
            }

            breadcrumbViewModel.Links.Add(new BreadcrumbLink { Name = page.Name }); 
            breadcrumbViewModel.CurrentPage = page;
            return breadcrumbViewModel;
        }
    }
}
