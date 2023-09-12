using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;
using Constants = Umbraco.Cms.Core.Constants;

namespace ThePensionsRegulator.Umbraco.Blocks
{
    public static class PublishedContentExtensions
    {
        /// <summary>
        /// Finds block lists that are direct children of an <see cref="IPublishedContent"/>.
        /// </summary>
        /// <param name="content">The <see cref="IPublishedContent"/> to search.</param>
        /// <returns>An IEnumerable of 0 or more matching block lists.</returns>
        public static IEnumerable<BlockListModel> FindBlockLists(this IPublishedContent content)
        {
            return content.FindBlockLists(null);
        }

        /// <summary>
        /// Finds block lists that are direct children of an <see cref="IPublishedContent"/>.
        /// </summary>
        /// <param name="content">The <see cref="IPublishedContent"/> to search.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>An IEnumerable of 0 or more matching block lists.</returns>
        public static IEnumerable<BlockListModel> FindBlockLists(this IPublishedContent content, IPublishedValueFallback? publishedValueFallback)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            if (content?.Properties is null)
            {
                throw new ArgumentException(nameof(content), $"{nameof(content)}.{nameof(content.Properties)} cannot be null");
            }

            return content.Properties
                .Where(x => x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList && x.HasValue())
                .Select(x => x.Value<BlockListModel>(publishedValueFallback ?? new NoopPublishedValueFallback()))
                .OfType<BlockListModel>();
        }

        /// <summary>
        /// Finds block grids that are direct children of an <see cref="IPublishedContent"/>.
        /// </summary>
        /// <param name="content">The <see cref="IPublishedContent"/> to search.</param>
        /// <returns>An IEnumerable of 0 or more matching block grids.</returns>
        public static IEnumerable<BlockGridModel> FindBlockGrids(this IPublishedContent content)
        {
            return content.FindBlockGrids(null);
        }

        /// <summary>
        /// Finds block grids that are direct children of an <see cref="IPublishedContent"/>.
        /// </summary>
        /// <param name="content">The <see cref="IPublishedContent"/> to search.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>An IEnumerable of 0 or more matching block grids.</returns>
        public static IEnumerable<BlockGridModel> FindBlockGrids(this IPublishedContent content, IPublishedValueFallback? publishedValueFallback)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            if (content?.Properties is null)
            {
                throw new ArgumentException(nameof(content), $"{nameof(content)}.{nameof(content.Properties)} cannot be null");
            }

            return content.Properties
                .Where(x => x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockGrid && x.HasValue())
                .Select(x => x.Value<BlockGridModel>(publishedValueFallback ?? new NoopPublishedValueFallback()))
                .OfType<BlockGridModel>();
        }

        /// <summary>
        /// Finds block lists and block grids that are direct children of an <see cref="IPublishedContent"/>.
        /// </summary>
        /// <param name="content">The <see cref="IPublishedContent"/> to search.</param>
        /// <returns>An IEnumerable of 0 or more matching block lists and block grids.</returns>
        public static IEnumerable<object> FindBlockModelCollections(this IPublishedContent content)
        {
            return content.FindBlockModelCollections(null);
        }

        /// <summary>
        /// Finds block lists and block grids that are direct children of an <see cref="IPublishedContent"/>.
        /// </summary>
        /// <param name="content">The <see cref="IPublishedContent"/> to search.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>An IEnumerable of 0 or more matching block lists and block grids.</returns>
        public static IEnumerable<object> FindBlockModelCollections(this IPublishedContent content, IPublishedValueFallback? publishedValueFallback)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            if (content?.Properties is null)
            {
                throw new ArgumentException(nameof(content), $"{nameof(content)}.{nameof(content.Properties)} cannot be null");
            }

            Func<IPublishedProperty, object?> select = x =>
            {
                return x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList ?
                                x.Value<BlockListModel>(publishedValueFallback ?? new NoopPublishedValueFallback()) as BlockModelCollection<BlockListItem> :
                                x.Value<BlockGridModel>(publishedValueFallback ?? new NoopPublishedValueFallback()) as BlockModelCollection<BlockGridItem>;
            };
            return content.Properties
                .Where(x => (x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList || x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockGrid) && x.HasValue())
                .Select(select)
                .OfType<object>();
        }
    }
}
