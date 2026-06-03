using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Umbraco.Blocks
{
    public static class PublishedElementExtensions
    {
        /// <summary>
        /// Finds block lists and block grids that are direct children of an <see cref="IPublishedElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="IPublishedElement"/> to search.</param>
        /// <returns>An IEnumerable of 0 or more matching block lists and block grids.</returns>
        public static IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> FindOverridableBlockModels(this IPublishedElement element)
        {
            return element.FindOverridableBlockModels(null);
        }

        /// <summary>
        /// Finds block lists and block grids that are direct children of an <see cref="IPublishedElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="IPublishedElement"/> to search.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>An IEnumerable of 0 or more matching block lists and block grids.</returns>
        public static IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> FindOverridableBlockModels(
            this IPublishedElement element, IPublishedValueFallback? publishedValueFallback)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            if (element?.Properties is null)
            {
                throw new ArgumentException(nameof(element), $"{nameof(element)}.{nameof(element.Properties)} cannot be null");
            }

            Func<IPublishedProperty, IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>?> select = x =>
            {
                return x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList ?
                                x.Value<OverridableBlockListModel>(publishedValueFallback ?? new NoopPublishedValueFallback()) :
                                x.Value<OverridableBlockGridModel>(publishedValueFallback ?? new NoopPublishedValueFallback());
            };
            return element.Properties
                .Where(x => (x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList || x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockGrid) && x.HasValue())
                .Select(select)
                .OfType<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>>();
        }

        /// <summary>
        /// Convert an <see cref="IPublishedElement"/> to the ModelsBuilder model for its element type
        /// </summary>
        /// <param name="element">The <see cref="IPublishedElement"/> to convert</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        public static T? AsPublishedElementModel<T>(this IPublishedElement element, IPublishedValueFallback? publishedValueFallback = null) where T : PublishedElementModel
            => Activator.CreateInstance(typeof(T), element, publishedValueFallback) as T;
    }
}
