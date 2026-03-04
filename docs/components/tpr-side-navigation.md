# TPR side navigation

A design component that is used to display a navigational menu on the left-hand side of a page.

## How it's implemented

Create your own implementation of the `ThePensionsRegulator.Frontend.Services.ITprSideNavigationLinksService` interface, which is part of the `ThePensionsRegulator.Frontend` NuGet package:

```csharp
public class SideNavigationLinksServiceForExampleApp : ITprSideNavigationLinksService
```

This allows you to provide your own navigational links to the TPR side navigation component. You simply need to implement the `public TprSideNavigationViewModel? GetLinks()` method in your custom service class and return a populated `ThePensionsRegulator.Frontend.Models.TprSideNavigationViewModel` object.

## Create your ITprSideNavigationLinksService implementation

Here is an example of this implementation from within the `Govuk.Frontend.Umbraco.ExampleApp`:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Services;
using ThePensionsRegulator.Umbraco;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.ExampleApp.Services
{
    public class SideNavigationLinksServiceForExampleApp : ITprSideNavigationLinksService
    {
        private IUmbracoPublishedContentAccessor _publishedContext;

        public SideNavigationLinksServiceForExampleApp(IUmbracoPublishedContentAccessor publishedContext)
        {
            _publishedContext = publishedContext;
        }

        public TprSideNavigationViewModel GetLinks()
        {
            var currentPage = _publishedContext.PublishedContent ?? throw new ArgumentNullException(nameof(_publishedContext.PublishedContent), "Published context is not initialised.");
            var rootNode = currentPage.Root().Children.FirstOrDefault(x => x.Name == "Side navigation") ?? currentPage.Root();
            TprSideNavigationViewModel sideNavigationViewModel = new() {
                TitleLink = new TprSideNavigationLink { Name = rootNode.Name, Url = rootNode.Url(), IsCurrentPage = (rootNode == currentPage) },
                NavigationLinks = CreateSideNavigationLinkChildren(currentPage, null, rootNode)
            };
            return sideNavigationViewModel;
        }

        private List<TprSideNavigationLink> CreateSideNavigationLinkChildren(IPublishedContent currentPage, TprSideNavigationLink? parent, IPublishedContent rootNode)
        {
            List<TprSideNavigationLink> children = new();

            if (rootNode.Children is not null && rootNode.Children.Any())
            {
                foreach (var child in rootNode.Children)
                {
                    var childNavItem = new TprSideNavigationLink
                    {
                        Name = child.Name,
                        Url = child.Url(),
                        IsCurrentPage = child == currentPage,
                        IsExpanded = currentPage.Ancestors().Contains(child) || child == currentPage,
                        Parent = parent
                    };
                    childNavItem.Children = CreateSideNavigationLinkChildren(currentPage, childNavItem, child);
                    children.Add(childNavItem);
                }
            }
           return children;
        }
    }
}
```

## Register your service

Modify your `Program.cs` as follows:

```csharp
services.AddTransient<ITprSideNavigationLinksService, SideNavigationLinksServiceForExampleApp>();
```

## Render the side navigation

You can then render the TPR side navigation component in your project using the `TprSideNavigation` partial view.

```razor
<partial name="TPR/TPRSideNavigation" />
```
