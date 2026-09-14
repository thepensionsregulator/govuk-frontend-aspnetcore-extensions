using System.Collections.Concurrent;

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
    internal interface IOverridablePublishedElementValueStore
    {
        IDictionary<string, object> Get(OverridablePublishedElement element);
    }

    internal sealed class OverridablePublishedElementValueStore : IOverridablePublishedElementValueStore
    {
        // An element may be referenced more than once during a request, so use its identity as the key.
        // ConcurrentDictionary also protects access if rendering touches the same model from concurrent code.
        private readonly ConcurrentDictionary<OverridablePublishedElement, IDictionary<string, object>> _values = new();

        public IDictionary<string, object> Get(OverridablePublishedElement element)
            => _values.GetOrAdd(element, _ => new ConcurrentDictionary<string, object>());
    }
}
