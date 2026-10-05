using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Core
{
    /// <inheritdoc/>
    public class DefaultOverridablePublishedElementFactory(IHttpContextAccessor _httpContextAccessor) : IOverridablePublishedElementFactory
    {
        /// <inheritdoc/>
        public IOverridablePublishedElement? Create(IPublishedElement? publishedElement)
            => publishedElement is null ? null : new OverridablePublishedElement(publishedElement, ResolveValueStore);

        private IOverridablePublishedElementValueStore ResolveValueStore()
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException($"No {nameof(HttpContext)} is available. {nameof(IOverridablePublishedElementValueStore)} can only be resolved by {nameof(DefaultOverridablePublishedElementFactory)} during an HTTP request.");

            return httpContext.RequestServices.GetRequiredService<IOverridablePublishedElementValueStore>();
        }
    }
}
