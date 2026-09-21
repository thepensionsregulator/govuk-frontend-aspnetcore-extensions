using System.Collections.Concurrent;

namespace ThePensionsRegulator.Umbraco.Core
{
    /// <inheritdoc />
    public class OverridablePublishedElementValueStore : IOverridablePublishedElementValueStore
    {
        // An element may be referenced more than once during a request, so use its identity as the key.
        // ConcurrentDictionary also protects access if rendering touches the same model from concurrent code.
        private readonly ConcurrentDictionary<OverridablePublishedElement, IDictionary<string, object>> _values = new();

        public IDictionary<string, object> Get(OverridablePublishedElement element)
            => _values.GetOrAdd(element, _ => new ConcurrentDictionary<string, object>());
    }
}
