namespace ThePensionsRegulator.Umbraco.Core
{
    /// <summary>
    /// Holds mutable overrides for the lifetime of the current ASP.NET request.
    /// </summary>
    /// <remarks>
    /// Published elements and their converted values can be cached by Umbraco beyond a single request.
    /// Keeping overrides in this scoped service prevents values written during one request from leaking into
    /// another request, including when requests execute concurrently.
    /// </remarks>
    public interface IOverridablePublishedElementValueStore
    {
        IDictionary<string, object> Get(OverridablePublishedElement element);
    }
}
