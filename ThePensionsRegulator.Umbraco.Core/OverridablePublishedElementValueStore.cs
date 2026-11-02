using System.Collections.Concurrent;

namespace ThePensionsRegulator.Umbraco.Core
{
    /// <inheritdoc />
    public class OverridablePublishedElementValueStore : IOverridablePublishedElementValueStore
    {
        // A published element may be wrapped more than once during a request, so use its stable key rather than
        // wrapper reference identity. ConcurrentDictionary also protects access if rendering touches the same model
        // from concurrent code.
        private readonly ConcurrentDictionary<Guid, IDictionary<string, object>> _values = new();

        /// <inheritdoc />
        public IDictionary<string, object> Get(Guid elementKey)
            => _values.GetOrAdd(elementKey, _ => new ConcurrentDictionary<string, object>());
    }
}
