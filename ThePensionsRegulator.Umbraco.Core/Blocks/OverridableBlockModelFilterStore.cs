using System.Collections.Concurrent;

namespace ThePensionsRegulator.Umbraco.Core.Blocks
{
    /// <inheritdoc />
    public class OverridableBlockModelFilterStore : IOverridableBlockModelFilterStore
    {
        private readonly ConcurrentDictionary<Guid, Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>> _filters = new();

        /// <inheritdoc />
        public bool TryGet(Guid modelKey, out Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>? filter)
        {
            var found = _filters.TryGetValue(modelKey, out var storedFilter);
            filter = storedFilter;
            return found;
        }

        /// <inheritdoc />
        public void Set(Guid modelKey, Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool> filter)
            => _filters[modelKey] = filter;
    }
}
