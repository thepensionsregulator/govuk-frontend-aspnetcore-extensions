using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Core
{
    /// <inheritdoc/>
    public class DefaultOverridablePublishedElementFactory(IOverridablePublishedElementValueStore _valueStore) : IOverridablePublishedElementFactory
    {
        /// <inheritdoc/>
        public IOverridablePublishedElement? Create(IPublishedElement? publishedElement)
            => publishedElement is null ? null : new OverridablePublishedElement(publishedElement, _valueStore);
    }
}