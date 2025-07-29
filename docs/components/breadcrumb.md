# Breadcrumb

The breadcrumbs component helps users to understand where they are within a website’s structure and move between levels.

## How it's implemented

The `govuk-frontend-aspnetcore-extensions` project supplies you with a service interface named `IGovUkBreadcrumbLinksService` in namespace `GovUk.Frontend.Umbraco.Services`, located inside the `ThePensionsRegulator.Govuk.Frontend.Umbraco` package. 

Create your own implementation of the `IGovUkBreadcrumbLinksService` interface by inheriting from it in your own custom service class e.g `public class BreadcrumbLinksServiceForExampleApp : IGovUkBreadcrumbLinksService`. This allows you to provide your own links to the GovUkBreadcrumb component. You can then have full control over the breadcrumb links in your project. You may benefit by:
-  Having different logic for generating breadcrumb links based on the page type.
-  Implementing custom logic for how to populate the link text of each link. 

You simply need to implement the `public BreadcrumbViewModel GetLinks(IPublishedContent Page)` method in your custom service class and return a populated `GovUk.Frontend.Umbraco.Models.BreadcrumbViewModel` object. 



## Create your IGovUkBreadcrumbLinksService implementation

Here is an example of this implementation from within the `GovUk.Frontend.Umbraco.ExampleApp`:
```csharp
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

```
## Register your service
You should register your service class within your Program/Startup file (or wherever you keep your services declarations) with code such as the following:

```csharp
services.AddTransient<IGovUkBreadcrumbLinksService, BreadcrumbLinksServiceForExampleApp>();
```

## Render the breadcrumb

You can then render the breadcrumb in your project by calling the `GovUkBreadcrumb` view component. 

```razor
@await Component.InvokeAsync("GovUkBreadcrumb")
```