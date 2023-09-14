using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;
using Constants = Umbraco.Cms.Core.Constants;

namespace ThePensionsRegulator.Umbraco.Blocks
{
    public static class PublishedContentExtensions
    {
        /// <summary>
        /// Finds block lists and block grids that are direct children of an <see cref="IPublishedContent"/>.
        /// </summary>
        /// <param name="content">The <see cref="IPublishedContent"/> to search.</param>
        /// <returns>An IEnumerable of 0 or more matching block lists and block grids.</returns>
        public static IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> FindOverridableBlockModels(this IPublishedContent content)
        {
            return content.FindOverridableBlockModels(null);
        }

        /// <summary>
        /// Finds block lists and block grids that are direct children of an <see cref="IPublishedContent"/>.
        /// </summary>
        /// <param name="content">The <see cref="IPublishedContent"/> to search.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>An IEnumerable of 0 or more matching block lists and block grids.</returns>
        public static IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> FindOverridableBlockModels(
            this IPublishedContent content, IPublishedValueFallback? publishedValueFallback)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            if (content?.Properties is null)
            {
                throw new ArgumentException(nameof(content), $"{nameof(content)}.{nameof(content.Properties)} cannot be null");
            }

            Func<IPublishedProperty, IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>?> select = x =>
            {
                return x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList ?
                                x.Value<OverridableBlockListModel>(publishedValueFallback ?? new NoopPublishedValueFallback()) :
                                x.Value<OverridableBlockGridModel>(publishedValueFallback ?? new NoopPublishedValueFallback());
            };
            return content.Properties
                .Where(x => (x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList || x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockGrid) && x.HasValue())
                .Select(select)
                .OfType<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>>();
        }
    }
}
