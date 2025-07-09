using Microsoft.AspNetCore.Mvc;
using System;
using ThePensionsRegulator.Frontend.Umbraco.Models;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco;

namespace GovUk.Frontend.Umbraco.ExampleApp.Components
{
    [ViewComponent(Name = "Breadcrumb")]
    public class RenderBreadcrumbComponent : ViewComponent
    {
        private BreadcrumbViewModel ViewModel;

        public RenderBreadcrumbComponent(IBreadcrumbLinksService breadcrumbLinksService, IUmbracoPublishedContentAccessor publishedContext)
        {
            if (breadcrumbLinksService is not null && publishedContext is not null)
            {
                ViewModel = breadcrumbLinksService.GetLinks(publishedContext.PublishedContent);
            }
            else if (breadcrumbLinksService is null)
            {
                ViewModel.Error = "Breadcrumb links service is not initialised.";
                throw new ArgumentNullException(nameof(breadcrumbLinksService), ViewModel.Error);
            }
            else
            {
                ViewModel.Error = "Published context is not initialised.";
                throw new ArgumentNullException(nameof(publishedContext), ViewModel.Error);
            }
        }

        public IViewComponentResult Invoke()
        {
            return View("Breadcrumb", ViewModel);
        }
    }
}
