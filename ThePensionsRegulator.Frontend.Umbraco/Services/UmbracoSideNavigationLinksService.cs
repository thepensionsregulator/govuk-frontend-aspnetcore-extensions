using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Services;
using ThePensionsRegulator.Umbraco.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    /// <summary>
    /// Gets links to display in side navigation based on Umbraco content. The root node is set to the ancestor of the current page, but can be overridden by inheriting classes.
    /// </summary>
    /// <remarks>Only nodes that are visible (i.e. not hidden in navigation) will be included in the side navigation links.</remarks>
    /// <param name="_contentAccessor">Accessor for the current Umbraco page.</param>
    /// <param name="_documentNavigationQueryService"></param>
    /// <param name="_publishedStatusFilteringService"></param>
    /// <param name="_publishedUrlProvider"></param>
    /// <param name="_publishedValueFallback"></param>
    public class UmbracoSideNavigationLinksService(
        IUmbracoPublishedContentAccessor _contentAccessor,
        IDocumentNavigationQueryService _documentNavigationQueryService,
        IPublishedContentStatusFilteringService _publishedStatusFilteringService,
        IPublishedUrlProvider _publishedUrlProvider,
        IPublishedValueFallback _publishedValueFallback) :
        ITprSideNavigationLinksService
    {
        /// <summary>
        /// Gets the root node to base side navigation upon. By default this is the ancestor of the current page, but inheriting classes can override this to provide a different root node.
        /// </summary>
        /// <returns>The root node to base navigation on, or <c>null</c> to suppress navigation.</returns>
        protected virtual IPublishedContent? GetRootContent() => _contentAccessor.PublishedContent.Root(_documentNavigationQueryService, _publishedStatusFilteringService);

        /// <summary>
        /// Gets the current Umbraco page.
        /// </summary>
        protected IPublishedContent CurrentPage => _contentAccessor.PublishedContent;

        /// <inheritdoc />
        public TprSideNavigationViewModel? GetLinks()
        {
            var rootNode = GetRootContent();
            if (rootNode is null || !rootNode.IsVisible(_publishedValueFallback)) { return null; }

            TprSideNavigationViewModel sideNavigationViewModel = new()
            {
                TitleLink = new TprSideNavigationLink { Name = rootNode.Name, Url = rootNode.Url(_publishedUrlProvider), IsCurrentPage = (rootNode == _contentAccessor.PublishedContent) },
                NavigationLinks = CreateSideNavigationLinkChildren(CurrentPage, null, rootNode)
            };
            return sideNavigationViewModel;
        }

        private List<TprSideNavigationLink> CreateSideNavigationLinkChildren(IPublishedContent currentPage, TprSideNavigationLink? parent, IPublishedContent rootNode)
        {
            List<TprSideNavigationLink> children = new();

            var childrenOfRoot = rootNode.Children(_documentNavigationQueryService, _publishedStatusFilteringService);
            if (childrenOfRoot is not null && childrenOfRoot.Any())
            {
                foreach (var child in childrenOfRoot)
                {
                    if (child is null || !child.IsVisible(_publishedValueFallback)) { continue; }
                    var childNavItem = new TprSideNavigationLink
                    {
                        Name = child.Name,
                        Url = child.Url(_publishedUrlProvider),
                        IsCurrentPage = child == currentPage,
                        IsExpanded = currentPage.Ancestors(_documentNavigationQueryService, _publishedStatusFilteringService).Contains(child) || child == currentPage,
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
