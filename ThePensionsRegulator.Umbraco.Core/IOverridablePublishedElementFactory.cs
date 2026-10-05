using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Core
{
    /// <summary>
    /// Wraps a read-only published element from Umbraco and allows its properties to be overridden with custom values for each request.
    /// </summary>
    public interface IOverridablePublishedElementFactory
    {
        /// <summary>
        /// Creates an <see cref="IOverridablePublishedElement"/> wrapping the specified <see cref="IPublishedElement"/>, using a request-scoped value store for overridden values.
        /// </summary>
        /// <param name="publishedElement">The published element to wrap.</param>
        /// <returns>An <see cref="IOverridablePublishedElement"/> wrapping the specified published element, or <c>null</c> if the input is <c>null</c>.</returns>
        IOverridablePublishedElement? Create([NotNullIfNotNull(nameof(publishedElement))] IPublishedElement? publishedElement);
    }
}