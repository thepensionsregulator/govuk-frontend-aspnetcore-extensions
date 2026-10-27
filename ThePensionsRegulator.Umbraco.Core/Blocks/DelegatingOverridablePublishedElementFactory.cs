using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Core.Blocks
{
    /// <summary>
    /// Wrap an older signature for a published element factory for backwards compatibility.
    /// </summary>
    /// <param name="_factory">The factory function.</param>
    internal sealed class DelegatingOverridablePublishedElementFactory(Func<IPublishedElement?, IOverridablePublishedElement?> _factory) : IOverridablePublishedElementFactory
    {
        public IOverridablePublishedElement? Create(IPublishedElement? publishedElement) => _factory(publishedElement);
    }
}