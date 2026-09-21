using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Core.Blocks
{
    /// <summary>
    /// Wrap an older signature for a published element factory for backwards compatibility.
    /// </summary>
    /// <param name="_factory">The factory function.</param>
    public sealed class DelegatingOverridablePublishedElementFactoryAccessor(Func<IPublishedElement?, IOverridablePublishedElement?> _factory) : IOverridablePublishedElementFactoryAccessor
    {
        public IOverridablePublishedElementFactory Get() => new DelegatingOverridablePublishedElementFactory(_factory);
    }
}