# TPR side navigation

A design component that is used to display a navigational menu on the left-hand side of a page.

## How it's implemented

If you are not using Umbraco, or the default implementation for Umbraco does not meet your needs, you can create your own implementation of the `ThePensionsRegulator.Frontend.Services.ITprSideNavigationLinksService` interface from the `ThePensionsRegulator.Frontend` NuGet package:

```csharp
using ThePensionsRegulator.Frontend.Services;
public class SideNavigationLinksServiceForExampleApp : ITprSideNavigationLinksService { }
```

You may benefit by:

- Having different logic for generating side navigation links based on the page type.
- Implementing custom logic for how to populate the link text of each link.

Alternatively inherit from the default Umbraco implementation:

```csharp
using ThePensionsRegulator.Frontend.Umbraco.Services;
public class SideNavigationLinksServiceForExampleApp : UmbracoSideNavigationLinksService { }
```

This allows you to override the `GetRootContent()` method to set a different start node.

## Register your service

Modify your `Program.cs` as follows:

```csharp
// if you are using Umbraco, remove the default implementation
services.Remove(builder.Services.First(builder => builder.ServiceType == typeof(ITprSideNavigationLinksService)));

// register your implementation
services.AddTransient<ITprSideNavigationLinksService, SideNavigationLinksServiceForExampleApp>();
```

## Render the side navigation

Render the TPR side navigation component on your view or layout using the `TPRSideNavigation` partial view.

```razor
<partial name="TPR/TPRSideNavigation" />
```

In Umbraco applications the TPR side navigation component is _not_ added to the block grid or block list.
