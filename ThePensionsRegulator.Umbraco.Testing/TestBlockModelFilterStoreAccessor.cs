using Moq;
using System.Collections.Concurrent;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;

namespace ThePensionsRegulator.Umbraco.Testing
{
    /// <summary>
    /// A test implementation of <see cref="IOverridableBlockModelFilterStoreAccessor"/> that can switch between request-scoped stores.
    /// </summary>
    public class TestBlockModelFilterStoreAccessor : IOverridableBlockModelFilterStoreAccessor
    {
        public TestBlockModelFilterStoreAccessor()
        {
            CurrentStore = CreateDefaultMockStore();
        }

        /// <summary>
        /// Gets or sets the current request-scoped filter store returned by <see cref="Get"/>.
        /// </summary>
        public IOverridableBlockModelFilterStore CurrentStore { get; set; }

        /// <inheritdoc/>
        public IOverridableBlockModelFilterStore Get() => CurrentStore;

        private static IOverridableBlockModelFilterStore CreateDefaultMockStore()
        {
            var filterDictionary = new ConcurrentDictionary<Guid, Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>>();

            var mock = new Mock<IOverridableBlockModelFilterStore>();
            mock.Setup(x => x.TryGet(It.IsAny<Guid>(), out It.Ref<Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>?>.IsAny))
                .Returns((Guid key, out Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>? filter) =>
                {
                    var found = filterDictionary.TryGetValue(key, out var storedFilter);
                    filter = storedFilter;
                    return found;
                });

            mock.Setup(x => x.Set(It.IsAny<Guid>(), It.IsAny<Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>>()))
                .Callback((Guid key, Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool> filter) =>
                {
                    filterDictionary[key] = filter;
                });

            return mock.Object;
        }
    }
}
