using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Blocks
{
    public static class PublishedElementExtensions
    {
        /// <summary>
        /// Convert an <see cref="IPublishedElement"/> to the ModelsBuilder model for its element type
        /// </summary>
        /// <param name="element">The <see cref="IPublishedElement"/> to convert</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        public static T? AsPublishedElementModel<T>(this IPublishedElement element, IPublishedValueFallback? publishedValueFallback = null) where T : PublishedElementModel
            => Activator.CreateInstance(typeof(T), element, publishedValueFallback) as T;
    }
}
