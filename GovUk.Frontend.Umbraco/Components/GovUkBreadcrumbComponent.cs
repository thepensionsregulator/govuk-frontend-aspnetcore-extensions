using Microsoft.AspNetCore.Mvc;
using System;
using GovUk.Frontend.Umbraco.Models;
using GovUk.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco;

namespace GovUk.Frontend.Umbraco.Components
{
    [ViewComponent(Name = "GovUkBreadcrumb")]
    public class GovUkBreadcrumbComponent : ViewComponent
    {
        private BreadcrumbViewModel ViewModel;

        public GovUkBreadcrumbComponent(IGovUkBreadcrumbLinksService breadcrumbLinksService, IUmbracoPublishedContentAccessor publishedContext)
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
