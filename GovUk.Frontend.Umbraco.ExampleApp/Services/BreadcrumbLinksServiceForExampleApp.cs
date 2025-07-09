using System.Linq;
using ThePensionsRegulator.Frontend.Umbraco.Models;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.ExampleApp.Services
{
    public class BreadcrumbLinksServiceForExampleApp : IBreadcrumbLinksService
    {
        public BreadcrumbViewModel GetLinks(IPublishedContent Page)
        {
            BreadcrumbViewModel breadcrumbViewModel = new();
            foreach (var ancestor in Page.Ancestors().OrderBy(x => x.Level)) 
            {
                breadcrumbViewModel.Ancestors.Add(new BreadcrumbLink { Name = ancestor.Name, Url = ancestor.Url() }); 
            }
            breadcrumbViewModel.CurrentPage = Page;
            return breadcrumbViewModel;
        }
    }
}
