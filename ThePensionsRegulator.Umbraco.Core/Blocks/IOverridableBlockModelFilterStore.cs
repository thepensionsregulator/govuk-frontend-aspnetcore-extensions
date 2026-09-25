namespace ThePensionsRegulator.Umbraco.Core.Blocks
{
    /// <summary>
    /// Holds mutable block filters for the lifetime of the current ASP.NET request.
    /// </summary>
    /// <remarks>
    /// Block models can be cached by Umbraco beyond a single request.
    /// Keeping filters in this scoped service prevents filters written during one request from leaking into
    /// another request, including when requests execute concurrently.
    /// </remarks>
    public interface IOverridableBlockModelFilterStore
    {
        bool TryGet(Guid modelKey, out Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>? filter);

        void Set(Guid modelKey, Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool> filter);
    }
}
