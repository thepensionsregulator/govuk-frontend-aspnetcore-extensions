# Breadcrumb

The breadcrumbs component helps users to understand where they are within a website’s structure and move between levels.

## How it's implemented

The `govuk-frontend-aspnetcore-extensions` project supplies you with a service interface named `IBreadcrumbLinksService`, located inside the `ThePensionsRegulator.Frontend.Umbraco` package. 

Create your own `BreadcrumbLinksService` by implementing the `IBreadcrumbLinksService` interface where you can decide how your project will collect the relevent names and urls of each pages ancestors to be listed on the breadcrumb within a `BreadcrumbViewModel`. 

You should register your `BreadcrumbLinksService` within your Program/Startup file (or wherever you keep your services) with a piece of code like this:

```csharp
services.AddTransient<IBreadcrumbLinksService, BreadcrumbLinksServiceForExampleApp>();
```

From there, you can integrate the `BreadcrumbLinksService` into a `ViewComponent` by creating a class responsible for rendering the breadcrumb component as well as a breadcrumb view for it to invoke.

Here is an example of this implementation from within the `GovUk.Frontend.Umbraco.ExampleApp`:

### BreadcrumbLinksService implementation

```csharp
﻿using System.Linq;
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
```

### Render breadcrumb ViewComponent

```csharp
﻿using Microsoft.AspNetCore.Mvc;
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
            }
            else
            {
                ViewModel.Error = "Published context is not initialised.";
            }
        }

        public IViewComponentResult Invoke()
        {
            return View("Breadcrumb", ViewModel);
        }
    }
}
```

### Breadcrumb view

```razor
﻿@using Umbraco.Cms.Core.Routing
@addTagHelper *, GovUk.Frontend.AspNetCore

@if (string.IsNullOrEmpty(Model.Error))
{
    <govuk-breadcrumbs collapse-on-mobile="true">

        @foreach (var item in Model.Ancestors)
        {
            <govuk-breadcrumbs-item href="@item.Url">@item.Name</govuk-breadcrumbs-item>
        }
        <govuk-breadcrumbs-item>@Model.CurrentPage.Name</govuk-breadcrumbs-item>

    </govuk-breadcrumbs>
}
else
{
    <p>@Model.Error</p>
}
```
