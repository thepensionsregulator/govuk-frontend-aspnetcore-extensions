# Breadcrumb

The breadcrumbs component helps users to understand where they are within a website’s structure and move between levels.

## How it's implemented

The `govuk-frontend-aspnetcore-extensions` project supplies you with service interfaces for breadcrumb management in namespace `GovUk.Frontend.Umbraco.Services`, located inside the `ThePensionsRegulator.Govuk.Frontend.Umbraco` package:

### Synchronous Implementation

Create your own implementation of the `IGovUkBreadcrumbLinksService` interface by inheriting from it in your own custom service class, e.g. `public class BreadcrumbLinksServiceForExampleApp : IGovUkBreadcrumbLinksService`. You simply need to implement the `public BreadcrumbViewModel GetLinks(IPublishedContent Page)` method.

### Asynchronous Implementation

For asynchronous operations (eg database calls, external API calls), create a class that implements `IGovUkAsyncBreadcrumbLinksService` by implementing the `public async Task<BreadcrumbViewModel> GetLinks(IPublishedContent Page)` method.

### Benefits

You can provide your own links to the GovUkBreadcrumb component and have full control over the breadcrumb links in your project:
-  Having different logic for generating breadcrumb links based on the page type.
-  Implementing custom logic for how to populate the link text of each link.
-  Using async operations to fetch breadcrumb data from external sources without blocking the request.

## Create your IGovUkBreadcrumbLinksService implementation (Synchronous)

Here is an example of a synchronous implementation from within the `GovUk.Frontend.Umbraco.ExampleApp`:

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

## Create your IGovUkAsyncBreadcrumbLinksService implementation (Asynchronous)

```csharp
using System.Linq;
using GovUk.Frontend.Umbraco.Models;
using GovUk.Frontend.Umbraco.Services;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.ExampleApp.Services
{
    public class AsyncBreadcrumbLinksServiceForExampleApp(IExternalBreadcrumbDataService externalService) : IGovUkAsyncBreadcrumbLinksService
    {
        public async Task<BreadcrumbViewModel> GetLinksAsync(IPublishedContent page)
        {
            BreadcrumbViewModel breadcrumbViewModel = new();

            foreach (var ancestor in page.Ancestors().OrderBy(x => x.Level)) 
            {
                // Example: fetch additional data from external service asynchronously
                var additionalData = await _externalService.GetLinkDataAsync(ancestor.Id);
                breadcrumbViewModel.Links.Add(new BreadcrumbLink 
                { 
                    Name = additionalData.DisplayName ?? ancestor.Name, 
                    Url = ancestor.Url() 
                }); 
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

**For synchronous implementation:**
```csharp
services.AddTransient<IGovUkBreadcrumbLinksService, BreadcrumbLinksServiceForExampleApp>();
```

**For asynchronous implementation:**
```csharp
services.AddTransient<IGovUkAsyncBreadcrumbLinksService, AsyncBreadcrumbLinksServiceForExampleApp>();
```

If both are registered, the asynchronous implementation will be used.

## Render the breadcrumb

You can then render the breadcrumb in your project by calling the `GovUkBreadcrumb` view component. 

```razor
@await Component.InvokeAsync("GovUkBreadcrumb")
```