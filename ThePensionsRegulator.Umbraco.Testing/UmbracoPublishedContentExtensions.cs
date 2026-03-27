using Moq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Umbraco.Testing
{
    public static class UmbracoPublishedContentExtensions
    {
        /// <summary>
        /// Sets up the <c>IPublishedContent.Parent()</c> extension method from the Umbraco.Extensions namespace.
        /// </summary>
        /// <param name="content">The child <see cref="IPublishedContent"/> instance.</param>
        /// <param name="queryService">The <see cref="IDocumentNavigationQueryService"/> instance.</param>
        /// <param name="filteringService">The <see cref="IPublishedContentStatusFilteringService"/> instance.</param>
        /// <param name="ancestors">The collection of ancestor <see cref="IPublishedContent"/> instances, which will be ordered by the <see cref="IPublishedContent.Level"/> property in descending order"/>.</param>
        /// <returns>The <see cref="Mock{IPublishedContent}"/> instance.</returns>
        private static Mock<IPublishedContent> SetupParent(this Mock<IPublishedContent> content, Mock<IDocumentNavigationQueryService> queryService, Mock<IPublishedContentStatusFilteringService> filteringService, IPublishedContent? parent)
        {
            var parentKey = parent?.Key;
            queryService
                .Setup(x => x.TryGetParentKey(content.Object.Key, out parentKey))
                .Returns(true);

            filteringService
                .Setup(x => x.FilterAvailable(
                    It.Is<IEnumerable<Guid>>(keys => keys.Count() == 0 && parentKey == null || keys.Count() == 1 && keys.First() == parentKey),
                    It.IsAny<string?>()))
                .Returns(parent is null ? [] : [parent]);

            return content;
        }

        /// <summary>
        /// Sets up the <c>IPublishedContent.Ancestors()</c> and <c>IPublishedContent.Parent()</c> extension methods from the Umbraco.Extensions namespace.
        /// </summary>
        /// <param name="content">The child <see cref="IPublishedContent"/> instance.</param>
        /// <param name="queryService">The <see cref="IDocumentNavigationQueryService"/> instance.</param>
        /// <param name="filteringService">The <see cref="IPublishedContentStatusFilteringService"/> instance.</param>
        /// <param name="ancestors">The collection of ancestor <see cref="IPublishedContent"/> instances, which will be ordered by the <see cref="IPublishedContent.Level"/> property in descending order"/>.</param>
        /// <returns>The <see cref="Mock{IPublishedContent}"/> instance.</returns>
        public static Mock<IPublishedContent> SetupAncestors(this Mock<IPublishedContent> content, Mock<IDocumentNavigationQueryService> queryService, Mock<IPublishedContentStatusFilteringService> filteringService, IEnumerable<IPublishedContent> ancestors)
        {
            var parent = ancestors.OrderByDescending(x => x.Level).FirstOrDefault();
            content.SetupParent(queryService, filteringService, parent);

            var ancestorKeys = ancestors.Select(a => a.Key);
            queryService
                .Setup(x => x.TryGetAncestorsKeys(content.Object.Key, out ancestorKeys))
                .Returns(true);

            filteringService
                .Setup(x => x.FilterAvailable(
                    It.Is<IEnumerable<Guid>>(keys => keys == ancestorKeys),
                    It.IsAny<string?>()))
                .Returns(ancestors.ToList());

            return content;
        }
    }
}
