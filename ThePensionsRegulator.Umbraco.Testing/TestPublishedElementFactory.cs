using Moq;
using System.Diagnostics.CodeAnalysis;
using ThePensionsRegulator.Umbraco.Core;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Testing
{
    /// <summary>
    /// A test implementation of <see cref="IOverridablePublishedElementFactory"/> that creates concrete <see cref="OverridablePublishedElement"/> instances for testing purposes,
    /// using a mock <see cref="IOverridablePublishedElementValueStore"/>.
    /// </summary>
    public class TestPublishedElementFactory : IOverridablePublishedElementFactory
    {
        private readonly Dictionary<Guid, IDictionary<string, object>> _values = new();

        public TestPublishedElementFactory() => ValueStore.Setup(x => x.Get(It.IsAny<Guid>()))
            .Returns((Guid elementKey) =>
            {
                if (!_values.TryGetValue(elementKey, out var propertyValues))
                {
                    propertyValues = new Dictionary<string, object>();
                    _values.Add(elementKey, propertyValues);
                }
                return propertyValues;
            });

        public Mock<IOverridablePublishedElementValueStore> ValueStore { get; } = new();

        /// <summary>
        /// Create a concrete <see cref="OverridablePublishedElement"/> instance for testing purposes, using a mock <see cref="IOverridablePublishedElementValueStore"/>
        /// </summary>
        /// <param name="publishedElement">The element to make overridable.</param>
        /// <returns>The created <see cref="IOverridablePublishedElement"/> instance, or <c>null</c> if the input was <c>null</c>.</returns>
        public IOverridablePublishedElement? Create([NotNullIfNotNull(nameof(publishedElement))] IPublishedElement? publishedElement)
        {
            if (publishedElement is null) { return null; }
            if (publishedElement is IOverridablePublishedElement) { return (IOverridablePublishedElement)publishedElement; }

            return new OverridablePublishedElement(publishedElement, () => ValueStore.Object);
        }
    }
}
