namespace ThePensionsRegulator.Umbraco.Core
{
    /// <summary>
    /// Resolves the current request's <see cref="IOverridablePublishedElementFactory"/> for use by singleton services
    /// that cannot inject the request-scoped factory directly.
    /// </summary>
    public interface IOverridablePublishedElementFactoryAccessor
    {
        /// <summary>
        /// Gets the <see cref="IOverridablePublishedElementFactory"/> registered for the current request.
        /// </summary>
        /// <returns>The current request's <see cref="IOverridablePublishedElementFactory"/>.</returns>
        IOverridablePublishedElementFactory Get();
    }
}
